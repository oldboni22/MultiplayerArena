using UnityEngine;



namespace BonBon
{

    public class CharStateDashing : CharStateClass
    {

        private Vector2 _dashDestination;
        private float _dashTime;

        public CharStateDashing(PlayerStateMachine machine, CharacterMovement controller, PlayerConstraints constraints) : base(machine, controller, constraints)
        {
        }

        protected override void CheckSwitchState()
        {
            if(_dashTime == 0)
            {
                if (_controller._moveVector == Vector2.zero)
                {
                    _machine.EnterState(CharState.Idle);
                }
                else
                {
                    _machine.EnterState(CharState.Running);
                }
            }
        }

        public override void FixedUpdate()
        {
            _controller.MoveCharacter(_constraints._dashSpeed, _dashDestination);
            _dashTime -= Time.fixedDeltaTime;
            if(_dashTime <= 0)
            {
                _dashTime = 0;
                _controller._isDashing.Value = false;
            }
        }

        internal override void EnterState()
        {
            Debug.Log("Entering dash");
            _dashDestination = _controller._moveVector;
            _dashTime = _constraints._dashDur;
        }

        internal override void ExitState()
        {
            
        }
    }
}