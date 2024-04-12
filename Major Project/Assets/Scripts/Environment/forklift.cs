using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forklift : MonoBehaviour, IConditional
{
    [SerializeField] float speed;
    [SerializeField] int numberRequiredToUnlock;
    [SerializeField] GameObject groundPos;
    [SerializeField] GameObject liftPos;
    [SerializeField] EventInteractable interactable;
    [SerializeField] bool goingUp;
    bool unlockedMachine;

    private void Start()
    {
        interactable.action += StartTheForklift;
    }
    void StartTheForklift()
    {
        StartCoroutine(moveTheFork());
    }
    IEnumerator moveTheFork()
    {
        while (true)
        {
            if (goingUp == false)
            {
                if (Vector3.Distance(transform.position, liftPos.transform.position) >= 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, liftPos.transform.position, speed);
                }
                else
                {
                    yield return new WaitForSeconds(2);
                    goingUp = true;
                }

            }
            else if (goingUp == true)
            {
                if (Vector2.Distance(transform.position, groundPos.transform.position) >= 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, groundPos.transform.position, speed);
                }
                else
                {
                    yield return new WaitForSeconds(2);
                    goingUp = false;
                }
            }
            yield return null;
        }
        
    }

    public bool Check()
    {
        return LevelOneManager.scrapParts == numberRequiredToUnlock;
    }
}
