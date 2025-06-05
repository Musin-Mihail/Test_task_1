using Animation.Scripts.Constants;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для управления активным снаряжением игрока (оружием).
    /// </summary>
    public interface IPlayerEquipment
    {
        void SetWeaponActive(WeaponType weaponType, bool isActive);
    }
}