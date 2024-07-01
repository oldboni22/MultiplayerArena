using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BonBon
{
    [CreateAssetMenu(menuName ="CharConstraints")]
    public class PlayerConstraintsCfg : ScriptableObject
    {

        [Header("Sprite")]
        [SerializeField] private float _scaleX;
        [SerializeField] private float _scaleY;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed;

        [Header("Stamina")]
        [SerializeField] private float _staminaRegen;
        [SerializeField] private float _staminaMax;
        [SerializeField] private float _staminaToDash;

        [Header("Dash")]
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashDur;

        public float MoveSpeed { get => _moveSpeed;}
        public float StaminaRegen { get => _staminaRegen;}
        public float StaminaMax { get => _staminaMax;}
        public float StaminaToDash { get => _staminaToDash;}
        public float DashSpeed { get => _dashSpeed;}
        public float DashDur { get => _dashDur;}
        public float ScaleX { get => _scaleX;  }
        public float ScaleY { get => _scaleY;  }
    }


}