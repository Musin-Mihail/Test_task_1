using UnityEngine;

namespace Animation.Scripts.Player
{
    public class PlayerFacade : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Transform ChestTransform { get; private set; }
        [field: SerializeField] public PlayerEquipment PlayerEquipment { get; private set; }
        [field: SerializeField] public GameObject Text { get; private set; }
    }
}