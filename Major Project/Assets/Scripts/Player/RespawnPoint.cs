using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public List<Pushable> pushableObjects = new List<Pushable>();
    bool gotSet;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Controls player))
        {
            if (player != null && !gotSet)
            {
                player.SetRespawnPoint(this.transform);
                gotSet = true;
                AudioManager.Instance.PlaySound(AudioManager.Instance.extraSFXSource, AudioManager.Instance.respawnSoundeffect);
            }
        }
    }
}
