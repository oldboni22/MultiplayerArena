using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BonBon
{

    public struct LobbyPlayerButtonParams
    {
        public string playerName;
        public string playerId;
    }

    public class LobbyPlayerButton : MonoBehaviour
    {
        [SerializeField] private Button _kickButton;
        [SerializeField] private TMP_Text _text;

        private ILobbyManager _lobbyManager;

        private string _playerId;

        [Inject]
        public void Inject(ILobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
        }

        private void Awake()
        {
            _kickButton.onClick.AddListener(KickPlayer);
        }

        public void SetUp(LobbyPlayerButtonParams @params)
        {
            _playerId = @params.playerId;

            _text.text = @params.playerName;
            _kickButton.gameObject.SetActive(_lobbyManager.IsHost());
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }

        private async void KickPlayer() 
        {
            
        }
    }
}