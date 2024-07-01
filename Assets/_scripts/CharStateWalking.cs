using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BonBon
{


    public class CharStateWalking : CharStateClass
    {
        public CharStateWalking(PlayerStateMachine machine, CharacterMovement controller, PlayerConstraints constraints) : base(machine, controller, constraints)
        {
        }



        protected override void CheckSwitchState()
        {
            if (_controller._moveVector == Vector2.zero)
            {
                _machine.EnterState(CharState.Idle);
            }
            else if (_controller._isDashing.Value is true)
                _machine.EnterState(CharState.Dashing);
        }

        public override void FixedUpdate()
        {
            _controller.MoveCharacter(_constraints._moveSpeed);
            _controller.AddStamina(_constraints._staminaRegen * .25f * Time.fixedDeltaTime);
        }

        internal override void EnterState()
        {
            Debug.Log("Entering Walking");
        }

        internal override void ExitState()
        {
            
        }
    }
}