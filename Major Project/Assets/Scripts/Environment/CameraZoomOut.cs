using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] float cameraSpeed;
    [SerializeField] Collider2D[] enterAreas;
    [SerializeField] Collider2D[] exitAreas;
    [SerializeField] Transform targetPosition;
    [SerializeField] float zoomOutSize;
    [SerializeField] Vector3 targetPosOffset;
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
        for (int i = 0; i < enterAreas.Length; i++)
        {
            if (zoomedOut)
            {
                virtualCamera.Follow = targetPosition;
                enterAreas[i].gameObject.SetActive(false);
                exitAreas[i].gameObject.SetActive(true);
            }
            else
            {
                targetPosition.position = transform.position + targetPosOffset;
                virtualCamera.Follow = gameObject.transform;
                virtualCamera.m_Lens.FieldOfView = originalCameraSize;
                enterAreas[i].gameObject.SetActive(true);
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
        foreach (Collider2D enterArea in enterAreas)
        {
            Gizmos.DrawWireCube(enterArea.bounds.center, enterArea.bounds.size);
        }

        Gizmos.color = Color.red;
        foreach (Collider2D exitArea in exitAreas)
        {
            Gizmos.DrawWireCube(exitArea.bounds.center, exitArea.bounds.size);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPosition.position, 2f);
    }
}
