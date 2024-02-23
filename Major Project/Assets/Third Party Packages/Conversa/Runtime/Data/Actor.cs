using UnityEngine;

namespace Conversa.Runtime
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Conversa/Actor")]
    public class Actor : ScriptableObject
    {
        [SerializeField] private string displayName;

        public string DisplayName
        {
            get => displayName;
            set => displayName = value;
        }
    }
}