using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;




namespace BonBon
{
    public enum CharDir
    {
        Up, Down, Left, Right
    }
    public enum CharState
    {
        Idle, Running, Dashing
    }

    public class CharacterAnimator : NetworkBehaviour
    {
        private GameObject _curState;
        [SerializeField] private string _colId;
        public string ColId => _colId;


        [SerializeField] private GameObject _up;
        [SerializeField] private GameObject _down;
        [SerializeField] private GameObject _left;
        [SerializeField] private GameObject _right;

        [SerializeField] private GameObject _dashUp;
        [SerializeField] private GameObject _dashDown;
        [SerializeField] private GameObject _dashLeft;
        [SerializeField] private GameObject _dashRight;

        [SerializeField] private GameObject _idle;

        private void Start()
        {
            _curState = _idle;
            _curState.SetActive(true);
        }



        public void OnVectorChanged(float x, float y, bool dash)
        {
            var vector = new Vector2(x, y);
            Debug.Log(vector);
            if (vector == Vector2.zero)
            {
                SetState(CharState.Idle, null);
                return;
            }


            var dir = DirManager.GetDir(vector);
            var state = dash? CharState.Dashing: CharState.Running;

            SetState(state, dir);
        }



        public void SetState(CharState state, CharDir? dir)
        {
            _curState?.SetActive(false);
            if (state == CharState.Idle)
                _curState = _idle;
            else if (state == CharState.Running)
                switch (dir)
                {
                    case CharDir.Up:
                        _curState = _up; break;
                    case CharDir.Down:
                        _curState = _down; break;
                    case CharDir.Left:
                        _curState = _left; break;
                    case CharDir.Right:
                        _curState = _right; break;
                }
            else 
            {
                switch (dir)
                {
                    case CharDir.Up:
                        _curState = _dashUp; break;
                    case CharDir.Down:
                        _curState = _dashDown; break;
                    case CharDir.Left:
                        _curState = _dashLeft; break;
                    case CharDir.Right:
                        _curState = _dashRight; break;
                }
            }
            _curState.SetActive(true);
        }
        

    }
}