using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace BonBon
{

    public interface IPlayerAnimatorsStorage
    {
        public CharacterAnimator GetAnimator(string colorId);
    }


    [CreateAssetMenu(menuName = "Storages/Animators")]
    public class PlayerAnimatorsStorage : ScriptableObject, IPlayerAnimatorsStorage
    {
        [SerializeField] private CharacterAnimator[] _animators;

        public CharacterAnimator GetAnimator(string colorId) => _animators.Where(a => a.ColId == colorId).FirstOrDefault();
    }
}