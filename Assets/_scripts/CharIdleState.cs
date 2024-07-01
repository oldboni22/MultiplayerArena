using UnityEngine;



namespace BonBon
{

    public class CharStateIdle : CharStateClass
    {
        public CharStateIdle(PlayerStateMachine machine, CharacterMovement controller, PlayerConstraints constraints) : base(machine, controller, constraints)
        {
        }

        protected override void CheckSwitchState()
        {
            if (_controller._moveVector != Vector2.zero)
                _machine.EnterState(CharState.Running);
        }

        public override void FixedUpdate()
        {
            _controller.AddStamina(_constraints._staminaRegen * Time.fixedDeltaTime);
        }

        internal override void EnterState()
        {
            Debug.Log("Entering Idle");
            _controller._rb.velocity = Vector2.zero;
            //_animator.SetState(CharState.Idle,null);
        }

        internal override void ExitState()
        {
            _controller._isDashing.Value = false;
        }
    }
}