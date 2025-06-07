using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за управление активным снаряжением игрока (оружием).
    /// </summary>
    public class PlayerEquipment : MonoBehaviour, IPlayerEquipment
    {
        public GameObject gun;
        public GameObject sword;

        public void SetWeaponActive(WeaponType weaponType, bool isActive)
        {
            switch (weaponType)
            {
                case WeaponType.Gun:
                    if (gun) gun.SetActive(isActive);
                    break;
                case WeaponType.Sword:
                    if (sword) sword.SetActive(isActive);
                    break;
                default:
                    Debug.LogWarning($"Unknown weapon type: {weaponType}");
                    break;
            }
        }
    }
}