using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BonBon
{


    public class PlayerStateMachine
    {
        internal CharacterMovement _controller;
        
        private CharStateClass _idle;
        private CharStateClass _walking;
        private CharStateClass _dashing;

        internal CharStateClass _curState;


        public void Update()
        {
            _curState.UpdateState();
        }
        public void FixedUpdate()
        {
            _curState.FixedUpdate();
        }

        public void EnterState(CharState state)
        {
            _curState?.ExitState();
            switch (state)
            {
                case CharState.Idle:
                    _curState = _idle; break;
                case CharState.Running: 
                    _curState = _walking; break;
                case CharState.Dashing:
                    _curState = _dashing; break;
            }
            _curState.EnterState();
        }
        public PlayerStateMachine(CharacterMovement controller, PlayerConstraints constraints)
        { 
            var factory = new PlayerStateFactory(controller,this ,constraints);

            _idle = factory.IdleState();
            _walking =factory.WalkingState();
            _dashing = factory.DashingState();
            
            EnterState(CharState.Idle);
        }

    }
}