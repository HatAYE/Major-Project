using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaDetection : MonoBehaviour
{
    [SerializeField] SirenStateMachine enemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemy.TransitionToState(EnemyState.Dialogue);
        }
    }
}
