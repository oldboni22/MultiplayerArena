using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;



namespace BonBon
{
    public interface ILobbyPlayerButtonPoolHandler
    {
        public void Clear();
        public void Add(LobbyPlayerButtonParams @params);
    }

    public class LobbyPlayerButtonPoolHandler : MonoBehaviour, ILobbyPlayerButtonPoolHandler
    {
        [SerializeField] private Transform _parent;
        private LobbyPlayerButtonMemoPool _pool;

        [Inject]
        public void Inject(LobbyPlayerButtonMemoPool pool)
        {
            _pool = pool;
            Debug.Log(_pool);
        }

        public void Add(LobbyPlayerButtonParams @params)
        {
            Debug.Log(_pool);

            var obj = _pool.Spawn(@params);

            obj.transform.SetParent(_parent);
            obj.transform.localScale = Vector3.one;
        }

        public void Clear()
        {
            _pool.DespawnAll();
        }
    }
}