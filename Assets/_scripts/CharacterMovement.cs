using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;



namespace BonBon
{
    public class CharacterMovement : NetworkBehaviour
    {
        [SerializeField] private float _maxStamina;
        [SerializeField] private Player _player;
        [SerializeField] internal Vector2 _moveVector;

        [SerializeField] internal Rigidbody2D _rb;
        internal NetworkVariable<bool> _isDashing = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        internal NetworkVariable<float> _curStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        internal NetworkVariable<float> _x = new NetworkVariable<float>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        internal NetworkVariable<float> _y = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        private PlayerConstraints _constraints;

        private PlayerStateMachine _playerStateMachine;
        private bool _isSetuped = false;

        [SerializeField] private CharacterAnimator _animator;

        public void SetUp(CharacterAnimator animator,PlayerStaminaBar staminaBar, PlayerConstraints constraints)
        {
            Debug.Log("SET UP " + OwnerClientId);

            _maxStamina = constraints._staminaMax;
            staminaBar.SetMax(_maxStamina);
            _curStamina.OnValueChanged += (float prev, float cur) => { staminaBar.UpdateTarget(cur); };


             
            _animator = animator;
            _x.OnValueChanged += (float prevX, float x) => { animator.OnVectorChanged(x, _y.Value, _isDashing.Value); };
            _y.OnValueChanged += (float prevY, float y) => { animator.OnVectorChanged(_x.Value, y, _isDashing.Value); };
            _isDashing.OnValueChanged += (bool prev, bool cur) => { animator.OnVectorChanged(_x.Value, _y.Value, cur); };

            _constraints = constraints;
            _playerStateMachine = new PlayerStateMachine(this, _constraints);

            _isSetuped = true;
        }

        async void Start()
        {
            if(IsOwner is false) 
                return;
            while (_isSetuped is false)
                await Task.Delay(50);
            _curStamina.Value = _maxStamina;
        }

        private void Update()
        {
            if (IsOwner is false || _isSetuped is false) return;
            
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            _moveVector = new Vector2(x, y);

            bool dash = _moveVector != Vector2.zero && Input.GetKeyDown(KeyCode.Space) && SpendStamina(_constraints._staminaToDash);
 
            if (_isDashing.Value is false)
            {
                if (dash)
                    _isDashing.Value = true;
                if (x != _x.Value)
                    _x.Value = x;
                if (y != _y.Value)
                    _y.Value = y;
            }

            
            _playerStateMachine.Update();
        }
        private void FixedUpdate()
        {
            if (IsOwner is false || _isSetuped is false) return;
            _playerStateMachine.FixedUpdate();
        }
        internal void MoveCharacter(float speed)
        {
            _rb.position += _moveVector * speed * Time.fixedDeltaTime;
        }
        internal void MoveCharacter(float speed, Vector2 dir)
        {
            _rb.position += dir * speed * Time.fixedDeltaTime;
        }
        internal bool SpendStamina(float stamina) 
        {
            if (IsOwner is false) return false;

            if (_curStamina.Value < stamina)
                return false;

            _curStamina.Value -= stamina;
            return true;
        }
        internal void AddStamina(float value)
        {  
            if (IsOwner is false) return;
            if (_curStamina.Value == _maxStamina)
                return;

            _curStamina.Value += value;
            if (_curStamina.Value > _maxStamina)
            {
                _curStamina.Value = _maxStamina;
            }
        }
    }

}