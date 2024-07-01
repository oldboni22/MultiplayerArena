using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BonBon
{


    public class PlayerStateFactory
    {
        private CharacterMovement _char;
        private PlayerStateMachine _machine;
        private PlayerConstraints _constraints;

        public PlayerStateFactory(CharacterMovement playerControllsController, PlayerStateMachine machine, PlayerConstraints constraints)
        {
            _char = playerControllsController;
            _machine = machine;
            _constraints = constraints;
        }


        internal CharStateClass IdleState() => new CharStateIdle(_machine, _char,_constraints);
        internal CharStateClass WalkingState() => new CharStateWalking(_machine, _char, _constraints);
        internal CharStateClass DashingState() => new CharStateDashing(_machine, _char, _constraints);

    }
}