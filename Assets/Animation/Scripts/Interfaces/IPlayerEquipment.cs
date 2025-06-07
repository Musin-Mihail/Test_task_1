using Animation.Scripts.Constants;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для управления активным снаряжением игрока (оружием).
    /// </summary>
    public interface IPlayerEquipment
    {
        /// <summary>
        /// Устанавливает активность указанного типа оружия.
        /// </summary>
        /// <param name="weaponType">Тип оружия.</param>
        /// <param name="isActive">Будет ли оружие активно.</param>
        void SetWeaponActive(WeaponType weaponType, bool isActive);
    }
}