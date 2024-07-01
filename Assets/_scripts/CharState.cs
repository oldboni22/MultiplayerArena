using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BonBon
{

    public abstract class CharStateClass
    {
        protected PlayerStateMachine _machine;
        protected CharacterMovement _controller;
        protected PlayerConstraints _constraints;

        internal abstract void EnterState();
        internal abstract void ExitState();
        public virtual void UpdateState() => CheckSwitchState();
        public virtual void FixedUpdate() { }
        protected abstract void CheckSwitchState();

        protected CharStateClass(PlayerStateMachine machine, CharacterMovement controller, PlayerConstraints constraints)
        {
            _machine = machine;
            _controller = controller;
            _constraints = constraints;
        }
    }

}

