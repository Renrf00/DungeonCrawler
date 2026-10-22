using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;
    // [SerializeField] private RectTransform Rotator;

    [Header("Movement")]
    [SerializeField] private float constantDeceleration = 0.0065f;
    [SerializeField] private float aimingDeceleration = 0.02f;
    [SerializeField] private Vector2 impulseForceRange = new Vector2(10, 30);
    [SerializeField] private float maxImpulsePress = 1.0f;
    private float impulsePress = 0;
    private Vector3 direction = Vector3.zero;

    [Header("Input")]
    [SerializeField] private KeyCode keyboardDashKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode mouseDashKey = KeyCode.Mouse0;
    private Vector2 mouseStartPosition = Vector2.zero;

    [HideInInspector] public bool canDestroy = false;

    private bool dashing;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(mouseDashKey))
        {

            if (Input.GetKeyDown(mouseDashKey))
            {
                mouseStartPosition = Camera.main.WorldToScreenPoint(transform.position);
            }

            if (Input.GetKey(mouseDashKey))
            {
                Decelerate(aimingDeceleration);
                GetDirection(mouseStartPosition);
                impulsePress += Time.deltaTime;
            }
            else
                Decelerate(constantDeceleration);

            if (Input.GetKeyUp(mouseDashKey))
            {
                dashing = true;
                // Rotator.gameObject.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKey(keyboardDashKey))
            {
                Decelerate(aimingDeceleration);
                GetDirection();
                impulsePress += Time.deltaTime;
            }
            else
                Decelerate(constantDeceleration);

            if (Input.GetKeyUp(keyboardDashKey))
            {
                dashing = true;
                // Rotator.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (dashing)
        {
            rb.AddForce(direction * getImpulseForce(), ForceMode.VelocityChange);
            impulsePress = 0;
            dashing = false;
        }
    }

    private float getImpulseForce()
    {
        float clampedImpulsePress = Mathf.Clamp(impulsePress, 0, maxImpulsePress);
        Debug.Log((clampedImpulsePress * (impulseForceRange.y - impulseForceRange.x) / maxImpulsePress) + impulseForceRange.x);
        return (clampedImpulsePress * (impulseForceRange.y - impulseForceRange.x) / maxImpulsePress) + impulseForceRange.x;
    }

    private void GetDirection()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // float angle = Mathf.Atan2(direction.z, direction.x);
        // Rotator.position = transform.position;
        // Vector3 rotation = new Vector3(0, angle * 180 / Mathf.PI + 90, 0);
        // Rotator.localEulerAngles = -rotation;

        // if (direction.x != 0 || direction.z != 0) Rotator.gameObject.SetActive(true);
        // else Rotator.gameObject.SetActive(false);

    }

    private void GetDirection(Vector2 mouseStartPosition)
    {
        Vector2 mouseCurrentPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Vector2 tempDirection = (mouseStartPosition - mouseCurrentPosition).normalized;

        float angle = Mathf.Atan2(tempDirection.y, tempDirection.x);

        if (angle % (Mathf.PI / 4) != 0)
        {
            angle = Mathf.Round(angle / (Mathf.PI / 4)) * (Mathf.PI / 4) - (Mathf.PI / 4);
            tempDirection = new Vector2(Mathf.Cos(angle) - Mathf.Sin(angle), Mathf.Sin(angle) + Mathf.Cos(angle));
        }

        direction = new Vector3(-tempDirection.x, 0, -tempDirection.y).normalized;

        // Rotator.position = transform.position;
        // Vector3 rotation = new Vector3(0, angle * 180 / Mathf.PI - 45, 0);
        // Rotator.localEulerAngles = -rotation;
        // Rotator.gameObject.SetActive(true);

    }

    private void Decelerate(float deceleration)
    {
        rb.linearVelocity -= rb.linearVelocity * deceleration;
    }
}
