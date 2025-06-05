using Animation.Scripts.Player;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для управления активным снаряжением игрока (оружием).
    /// </summary>
    public interface IPlayerEquipment
    {
        void SetWeaponActive(PlayerEquipment.WeaponType weaponType, bool isActive);
    }
}