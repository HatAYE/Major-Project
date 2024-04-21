using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueShift : MonoBehaviour
{
    [SerializeField] Essence[] essences;
    [SerializeField] float maxDistance = 100f;
    [SerializeField] Color nearColor = Color.red; 
    [SerializeField] Color farColor = Color.blue; 
    [SerializeField] float transitionSpeed = 50f;

    private Camera cam;
    private Color initialColor;

    void Start()
    {
        cam = GetComponent<Camera>();
        initialColor = cam.backgroundColor;

        essences = FindObjectsOfType<Essence>();
    }

    void Update()
    {
        
        float closestDistance = Mathf.Infinity;
        foreach (Essence essence in essences)
        {
            if (essence!=null)
            {
                float distance = Vector3.Distance(transform.position, essence.transform.position);
                closestDistance = Mathf.Min(closestDistance, distance);
            }
            
        }

        float lerpValue = Mathf.Clamp01(closestDistance / maxDistance);

        Color lerpedColor = Color.Lerp(nearColor, farColor, lerpValue);

        cam.backgroundColor = Color.Lerp(cam.backgroundColor, lerpedColor, Time.deltaTime * transitionSpeed);
    }
}
