using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShrinkingAndEnlarging : MonoBehaviour
{
    public Vector2 currentSize;
    Vector2 shrinkingSize;
    Vector2 regularSize;
    public Vector2 largeSize;
    [SerializeField] Sprite[] sizeUI = new Sprite[3];
    [SerializeField] Image uiObject;
    void Start()
    {
        regularSize= transform.localScale;
        currentSize = regularSize;
        shrinkingSize = regularSize / 2f;
        largeSize = regularSize * 2;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //move to previous size
                if (currentSize == shrinkingSize)
                {
                    return;
                }
                else if (currentSize == regularSize)
                {
                    uiObject.sprite = sizeUI[0];
                    SwitchSize(shrinkingSize);
                }
                else if (currentSize == largeSize)
                {
                    uiObject.sprite = sizeUI[1];
                    SwitchSize(regularSize);
                    GetComponent<Controls>().currentPushingRange = GetComponent<Controls>().originalPushingRange;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                //move to next size
                if (currentSize == shrinkingSize)
                {
                    uiObject.sprite = sizeUI[1];
                    SwitchSize(regularSize);
                    GetComponent<Controls>().currentPushingRange = GetComponent<Controls>().originalPushingRange;
                }
                else if (currentSize == regularSize)
                {
                    uiObject.sprite = sizeUI[2];
                    SwitchSize(largeSize);
                    GetComponent<Controls>().currentPushingRange = GetComponent<Controls>().enlargedPushingRange;
                }
                else if (currentSize == largeSize)
                {
                    return;
                }
            }
        }

    }
    void SwitchSize(Vector2 newSize)
    {
        currentSize = newSize;
        transform.localScale = currentSize;
    }
}
