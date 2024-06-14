using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] float totalTime;
    [SerializeField] Collider2D[] enterAreas;
    [SerializeField] Collider2D[] secondEntryAreas;
    [SerializeField] Collider2D[] exitAreas;
    [SerializeField] Transform rightTargetPosition;
    [SerializeField] Vector3 rightTargetPosOffset;
    [SerializeField] Vector3 leftTargetPosOffset;
    [SerializeField] Transform leftTargetPosition;
    [SerializeField] float zoomOutSize=100;
    bool zoomedOut;
    float originalCameraSize { get; set; }
    CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera= FindObjectOfType<CinemachineVirtualCamera>();
        originalCameraSize = virtualCamera.m_Lens.FieldOfView;
        virtualCamera.Follow = transform;
    }
    private void Update()
    {
        /*rightTargetPosition.position = transform.position + rightTargetPosOffset;
        leftTargetPosition.position= transform.position + leftTargetPosOffset;*/
        for (int i = 0; i < exitAreas.Length; i++)
        {
            if (zoomedOut)
            {
                if (i < enterAreas.Length)
                {
                    enterAreas[i].gameObject.SetActive(false);
                    secondEntryAreas[i].gameObject.SetActive(false);
                }
                exitAreas[i].gameObject.SetActive(true);
            }
            else
            {
                //virtualCamera.Follow = gameObject.transform;
                //virtualCamera.m_Lens.FieldOfView = originalCameraSize;
                if (i <enterAreas.Length)
                {
                    enterAreas[i].gameObject.SetActive(true);
                    secondEntryAreas[i].gameObject.SetActive(true);
                }
                exitAreas[i].gameObject.SetActive(false);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (Collider2D enterArea in enterAreas)
        {
            if (other == enterArea)
            {
                //virtualCamera.Follow = rightTargetPosition;
                StartCoroutine(ZoomCamera(zoomOutSize, rightTargetPosition));
                zoomedOut = true;
                return;
            }
        }

        foreach (Collider2D secondEntry in secondEntryAreas)
        {
            if (other == secondEntry)
            {
                //virtualCamera.Follow = leftTargetPosition;
                StartCoroutine(ZoomCamera(zoomOutSize, leftTargetPosition));
                zoomedOut = true;
                return;
            }
        }

        foreach (Collider2D exitArea in exitAreas)
        {
            if (other == exitArea)
            {
                StartCoroutine(ZoomCamera(originalCameraSize, transform));
                zoomedOut = false;
                return;
            }
        }
        
    }
    //float time;
    IEnumerator ZoomCamera(float targetZoom, Transform followTarget)
    {
        float startZoom = virtualCamera.m_Lens.FieldOfView;
        float elapsedTime = 0f;

        virtualCamera.Follow = followTarget;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(startZoom, targetZoom, elapsedTime / totalTime);
            yield return null;
        }

        virtualCamera.m_Lens.FieldOfView = targetZoom;
        zoomedOut = targetZoom == zoomOutSize;
    }
    /*IEnumerator ZoomCamera(float zoomSize)
    {
        while (time < totalTime)
        {
            time += Time.deltaTime;
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, zoomSize, time / totalTime);
            yield return null;
        }
        time = 0;
    }*/

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        for(int i=0; i < enterAreas.Length; i++)
        {
            if (enterAreas[i] != null)
                Gizmos.DrawWireCube(enterAreas[i].bounds.center, enterAreas[i].bounds.size);
            if (secondEntryAreas[i] != null)
            Gizmos.DrawWireCube(secondEntryAreas[i].bounds.center, secondEntryAreas[i].bounds.size);
        }

        Gizmos.color = Color.red;
        foreach (Collider2D exitArea in exitAreas)
        {
            Gizmos.DrawWireCube(exitArea.bounds.center, exitArea.bounds.size);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(rightTargetPosition.position, 2f);
        Gizmos.DrawWireSphere(leftTargetPosition.position, 2f);
    }
}
