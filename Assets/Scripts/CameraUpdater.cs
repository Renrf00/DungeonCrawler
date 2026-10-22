using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class CameraUpdater : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private float doTweenDuration = 2;

    private void Start()
    {
        StartCoroutine(UpdateCameraPos());
    }

    private IEnumerator UpdateCameraPos()
    {
        while (gameObject)
            yield return transform.DOMove(new Vector3(playerPos.position.x, playerPos.position.y + 20, playerPos.position.z), doTweenDuration);
    }
}
