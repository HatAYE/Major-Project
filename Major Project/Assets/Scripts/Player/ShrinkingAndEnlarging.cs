using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingAndEnlarging : MonoBehaviour
{
    public Vector2 currentSize;
    Vector2 shrinkingSize;
    Vector2 regularSize;
    public Vector2 largeSize;
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
                    SwitchSize(shrinkingSize);
                }
                else if (currentSize == largeSize)
                {
                    SwitchSize(regularSize);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                //move to next size
                if (currentSize == shrinkingSize)
                {
                    SwitchSize(regularSize);
                }
                else if (currentSize == regularSize)
                {
                    SwitchSize(largeSize);
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
