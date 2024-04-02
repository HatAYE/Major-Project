using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelDrill : MonoBehaviour
{
    [SerializeField] int numberRequiredToUnlock;
    bool unlockedMachine;
    Animation animation;
    private void Start()
    {
        animation = GetComponent<Animation>();
    }
    void Update()
    {
        if (unlockedMachine)
        {
            StartCoroutine(ActivateDrill());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Controls player))
        {
            if (LevelOneManager.scrapParts == numberRequiredToUnlock)
            {
                if (!unlockedMachine)
                {
                    LevelOneManager.scrapParts -= numberRequiredToUnlock;
                    unlockedMachine = true;
                }
            }
        }
    }
    IEnumerator ActivateDrill()
    {
        GameManager.currenState = gameStates.frozen;
        animation.Play();
        yield return new WaitForSeconds(5.5f);
        GameManager.currenState = gameStates.playing;
        Destroy(gameObject);
    }
}
