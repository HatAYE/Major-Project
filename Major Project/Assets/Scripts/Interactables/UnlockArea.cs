using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockArea : MonoBehaviour
{
    [SerializeField] ElementalPlate[] plates;
    BlockAreas blockAreas;
    [SerializeField] int numOfSolvedPlates;
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
            if (plates[i].solved == false)
            {
                return;
            }
            if (plates[i].solved == true && !plates[i].gotChecked)
            {
                numOfSolvedPlates++;
                plates[i].gotChecked = true;
                return;

            }

            if (numOfSolvedPlates== plates.Length)
            {
                blockAreas.UnlockArea();
                //give second memory
                enabled = false;
            }
            
        }
    }
}
