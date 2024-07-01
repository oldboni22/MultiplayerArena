using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace BonBon
{ 
    public struct PlayerConstraints : INetworkSerializeByMemcpy
    {

        public  float _scaleX, _scaleY;

        public  float _moveSpeed;

        public  float _staminaRegen;
        public  float _staminaMax;
        public  float _staminaToDash;

        public  float _dashSpeed;
        public  float _dashDur;

        public PlayerConstraints(PlayerConstraintsCfg cfg)
        {
            _dashDur = cfg.DashDur;
            _moveSpeed = cfg.MoveSpeed;
            _dashSpeed = cfg.DashSpeed;
            _staminaMax = cfg.StaminaMax;
            _staminaRegen = cfg.StaminaRegen;
            _staminaToDash = cfg.StaminaToDash;
            _scaleX = cfg.ScaleX;
            _scaleY = cfg.ScaleY;
        }
        public PlayerConstraints(PlayerConstraints cfg)
        {
            _dashDur = cfg._dashDur;
            _moveSpeed = cfg._moveSpeed;
            _dashSpeed = cfg._dashSpeed;
            _staminaMax = cfg._staminaMax;
            _staminaRegen = cfg._staminaRegen;
            _staminaToDash = cfg._staminaToDash;
            _scaleX = cfg._scaleX;
            _scaleY = cfg._scaleY;
        }

        //public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        //{
        //    serializer.SerializeValue(ref _scaleX);
        //    serializer.SerializeValue(ref _scaleY);
        //    serializer.SerializeValue(ref _dashDur);
        //    serializer.SerializeValue(ref _moveSpeed);
        //    serializer.SerializeValue(ref _staminaRegen);
        //    serializer.SerializeValue(ref _dashSpeed);
        //    serializer.SerializeValue(ref _staminaMax);
        //    serializer.SerializeValue(ref _staminaToDash);
        //}
        //public PlayerConstraints() { }
    }
}