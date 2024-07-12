using BonBon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Zenject;


namespace BonBon
{


    public class LobbyButtonMemoPool : MonoMemoryPool<string,string, LobbyButton>
    {
        private readonly List<LobbyButton> _buttons = new List<LobbyButton>(5);

        protected override void OnCreated(LobbyButton item)
        {
            _buttons.Add(item);
        }
        protected override void OnDespawned(LobbyButton item)
        {
            item.Clear();
        }
        protected override void Reinitialize(string lobbyId,string lobbyName, LobbyButton item)
        {
            item.SetUp(lobbyId, lobbyName);
            base.Reinitialize(lobbyId,lobbyName, item);
        }

        public void DespawnAll()
        {
            foreach (var item in _buttons)
            {
                if(item.gameObject.activeInHierarchy)
                    Despawn(item);
            }
        }
    }
}