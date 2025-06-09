using Animation.Scripts.Constants;
using UnityEngine;

namespace Animation.Scripts.Player
{
    public interface IPlayerEquipment
    {
        /// <summary>
        /// Устанавливает активность указанного типа оружия.
        /// </summary>
        /// <param name="weaponType">Тип оружия.</param>
        /// <param name="isActive">Будет ли оружие активно.</param>
        void SetWeaponActive(WeaponType weaponType, bool isActive);
    }

    public class PlayerEquipment : MonoBehaviour, IPlayerEquipment
    {
        [SerializeField] private GameObject gun;
        [SerializeField] private GameObject sword;

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
            }
        }
    }
}