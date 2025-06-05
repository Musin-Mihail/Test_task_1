using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за управление активным снаряжением игрока (оружием).
    /// Реализует интерфейс IPlayerEquipment.
    /// </summary>
    public class PlayerEquipment : MonoBehaviour, IPlayerEquipment
    {
        public GameObject gun;
        public GameObject sword;

        public enum WeaponType
        {
            Gun,
            Sword
        }

        /// <summary>
        /// Устанавливает активность указанного типа оружия.
        /// </summary>
        /// <param name="weaponType">Тип оружия.</param>
        /// <param name="isActive">Будет ли оружие активно.</param>
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