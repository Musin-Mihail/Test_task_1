using System;
using System.Collections.Generic;
using Animation.Scripts.Constants;
using UnityEngine;

namespace Animation.Scripts.Player
{
    public interface IPlayerEquipment
    {
        void SetWeaponActive(WeaponType weaponType, bool isActive);
    }

    [Serializable]
    public class WeaponMapping
    {
        public WeaponType type;
        public GameObject weaponObject;
    }

    public class PlayerEquipment : MonoBehaviour, IPlayerEquipment
    {
        [SerializeField] private List<WeaponMapping> weaponMappings;

        private Dictionary<WeaponType, GameObject> _weaponDictionary;

        private void Awake()
        {
            _weaponDictionary = new Dictionary<WeaponType, GameObject>();
            foreach (var mapping in weaponMappings)
            {
                if (mapping.weaponObject)
                {
                    _weaponDictionary[mapping.type] = mapping.weaponObject;
                }
            }
        }

        public void SetWeaponActive(WeaponType weaponType, bool isActive)
        {
            if (_weaponDictionary.TryGetValue(weaponType, out var weaponObject))
            {
                weaponObject.SetActive(isActive);
            }
            else
            {
                Debug.LogWarning($"Weapon of type {weaponType} is not registered in PlayerEquipment.");
            }
        }
    }
}