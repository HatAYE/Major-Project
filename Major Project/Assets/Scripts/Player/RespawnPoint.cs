using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Controls player))
        {
            if (player != null)
            {
                player.SetRespawnPoint(this.transform);
            }
        }
    }
}
