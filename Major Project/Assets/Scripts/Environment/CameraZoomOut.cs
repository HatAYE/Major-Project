using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] float cameraSpeed;
    [SerializeField] Collider2D[] enterAreas;
    [SerializeField] Collider2D[] secondEntryAreas;
    [SerializeField] Collider2D[] exitAreas;
    [SerializeField] Transform rightTargetPosition;
    [SerializeField] Vector3 rightTargetPosOffset;
    [SerializeField] Transform leftTargetPosition;
    [SerializeField] Vector3 leftTargetPosOffset;
    [SerializeField] float zoomOutSize=100;
    bool zoomedOut;
    float originalCameraSize { get; set; }
    CinemachineVirtualCamera virtualCamera;
    //when u enter trigger, move camera to position
    //when you exit, move camera to original postiion
    //FIX THE CAMERA FREEZE WHEN IT ZOOMES OUT

    void Start()
    {
        virtualCamera= FindObjectOfType<CinemachineVirtualCamera>();
        originalCameraSize = virtualCamera.m_Lens.FieldOfView;
    }
    private void Update()
    {
        rightTargetPosition.position = transform.position + rightTargetPosOffset;
        leftTargetPosition.position= transform.position + leftTargetPosOffset;
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
                virtualCamera.Follow = gameObject.transform;
                virtualCamera.m_Lens.FieldOfView = originalCameraSize;
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
                virtualCamera.Follow = rightTargetPosition;
                StartCoroutine(ZoomCamera(zoomOutSize));
                zoomedOut = true;
                return;
            }
        }

        foreach (Collider2D secondEntry in secondEntryAreas)
        {
            virtualCamera.Follow = leftTargetPosition;
            if (other == secondEntry)
            {
                StartCoroutine(ZoomCamera(zoomOutSize));
                zoomedOut = true;
                return;
            }
        }

        foreach (Collider2D exitArea in exitAreas)
        {
            if (other == exitArea)
            {
                StartCoroutine(ZoomCamera(originalCameraSize));
                zoomedOut = false;
                return;
            }
        }
        
    }

    IEnumerator ZoomCamera(float zoomSize)
    {
        while (Mathf.Abs(virtualCamera.m_Lens.FieldOfView - zoomSize) > 0.01f)
        {
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, zoomSize, cameraSpeed * Time.deltaTime);


            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int i=0; i < enterAreas.Length; i++)
        {
            Gizmos.DrawWireCube(enterAreas[i].bounds.center, enterAreas[i].bounds.size);
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
