using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BonBon
{


    [CreateAssetMenu(menuName ="GlobalInstaller")]
    public class GlobalInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private ConstraintManager _constraintManager;
        [SerializeField] private PlayerAnimatorsStorage _animatorsStorage;
        public override void InstallBindings()
        {
            Container.Bind<IPlayerAnimatorsStorage>().To<PlayerAnimatorsStorage>().FromInstance(_animatorsStorage).AsCached().NonLazy();
            Container.Bind<IConstraintManager>().To<ConstraintManager>().FromInstance(_constraintManager).AsCached().NonLazy();

        }
    }
}