
using UnityEngine;

namespace BonBon
{
    public interface IConstraintManager
    {
        public PlayerConstraintsCfg GetDefault();
    }

    [CreateAssetMenu(menuName = "Constraints")]
    public class ConstraintManager : ScriptableObject, IConstraintManager
    {

        [SerializeField] private PlayerConstraintsCfg _default;

        public PlayerConstraintsCfg GetDefault()
        {
            return _default;
        }
    }
}