using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Zenject;

namespace BonBon
{
    public interface ILobbyMemoPoolHandler
    {
        public void Clear();
        public void Add(string lobbyId, string lonnyName);
    }

    public class LobbyMemoPoolHandler : MonoBehaviour, ILobbyMemoPoolHandler
    {
        [SerializeField] private Transform _parent;
        private LobbyButtonMemoPool _pool;

        [Inject]
        public void Inject(LobbyButtonMemoPool pool)
        {
            _pool = pool;
            Debug.Log(_pool);
        }

        public void Add(string lobbyId,string lonnyName)
        {
            Debug.Log(_pool);

            var obj = _pool.Spawn(lobbyId,lonnyName);
            
            obj.transform.SetParent(_parent);
            obj.transform.localScale = Vector3.one;
        }

        public void Clear()
        {
            _pool.DespawnAll();
        }
    }
}