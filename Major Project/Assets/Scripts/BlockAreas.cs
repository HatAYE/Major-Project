using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAreas : MonoBehaviour
{
    GameObject[] blockAreas;
    Controls player;
    void Start()
    {
        blockAreas = GameObject.FindGameObjectsWithTag("block");
        player= GetComponent<Controls>();
        UnlockArea();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.inCombat)
        {
            BlockArea();
        }
        else if(!player.inCombat)
        {
            UnlockArea();
        }
    }
    void BlockArea()
    {
        if (blockAreas != null)
        {
            foreach (GameObject area in blockAreas)
            {
                area.SetActive(true);
            }
        }
    }

    void UnlockArea()
    {
        if (blockAreas != null)
        {
            foreach (GameObject area in blockAreas)
            {
                area.SetActive(false);
            }
        }
    }
}
