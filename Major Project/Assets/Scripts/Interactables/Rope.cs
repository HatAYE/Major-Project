using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    Rigidbody2D hook;
    [SerializeField] GameObject ropePrefab;
    [SerializeField] int numLinks=5;

    HingeJoint2D top;
    Controls player;
    void Start()
    {
        player = FindObjectOfType<Controls>();
        hook= transform.GetChild(0).GetComponent<Rigidbody2D>();
        Rigidbody2D prevBod = hook;
        for (int i = 0; i < numLinks; i++)
        {
            GameObject newSeg = Instantiate(ropePrefab);
            newSeg.transform.parent = transform;
            newSeg.transform.position = transform.position;
            HingeJoint2D hj= newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody =prevBod;

            prevBod=newSeg.GetComponent<Rigidbody2D>();

            if(i==0)
            {
                top = hj;
            }
        }
    }
    /*void AddLink()
    {
        GameObject newLink = Instantiate(ropePrefab);
        newLink.transform.parent = transform;
        newLink.transform.position = transform.position;
        HingeJoint2D hj = newLink.GetComponent<HingeJoint2D>();
        hj.connectedBody = hook;
        newLink.GetComponent<RopeSegment>().connectedBelow = top.gameObject;
        top.connectedBody= newLink.GetComponent<Rigidbody2D>();
        top.GetComponent<RopeSegment>().ResetAnchor();
        top = hj;
    }

    void RemoveLink()
    {
        if(top.gameObject.GetComponent<RopeSegment>().isPlayerAttached)
        {
            player.Slide(-1);
        }
        HingeJoint2D newTop= top.gameObject.GetComponent<RopeSegment>().connectedBelow.GetComponent<HingeJoint2D>();
        newTop.connectedBody = hook;
        newTop.gameObject.transform.position=hook.gameObject.transform.position;
        newTop.GetComponent<RopeSegment>().ResetAnchor();
        Destroy(top.gameObject);
        top = newTop;
    }*/
    // Update is called once per frame
    void Update()
    {
        
    }
}
