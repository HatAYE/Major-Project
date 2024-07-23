using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockArea : MonoBehaviour
{
    ElementalPlate[] plates;
    BlockAreas blockAreas;
    void Start()
    {
        blockAreas= FindObjectOfType<BlockAreas>();
        plates=FindObjectsOfType<ElementalPlate>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < plates.Length; i++)
        {
            if (plates[i].solved==false)
            {
                return;
            }
            blockAreas.UnlockArea();
            //give second memory
        }
    }
}
