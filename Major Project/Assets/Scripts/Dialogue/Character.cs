using Conversa.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Conversa/Character")]
public class Character : Actor
{
    [SerializeField] Sprite avatar;

    public Sprite Avatar => avatar;
}