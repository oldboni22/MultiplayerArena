using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using Zenject;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using System.Threading.Tasks;

namespace BonBon
{

    public interface ILobbiesUiManager
    {
        public void DisplayLobby(Lobby lobby);
        public void DisplaySeeker(IEnumerable<Lobby> lobbies);
        public void BlockStartButton(float dur);
        public void UpdateState(LobbyManagerState state);
    }

    public class LobbiesUiManager : MonoBehaviour, ILobbiesUiManager
    {
        private ILobbyMemoPoolHandler _poolHandler;
        private ILobbyPlayerButtonPoolHandler _playerButtonspoolHandler;

        [Header("Canvas")]
        [SerializeField] private GameObject _seekerCanvas;
        [SerializeField] private GameObject _lobbyCanvas;

        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private TMP_Text _startButtonText;


        [Inject]
        public void Inject(ILobbyMemoPoolHandler poolHandler, ILobbyPlayerButtonPoolHandler lobbyPlayerButtonPoolHandler)
        {
            _poolHandler = poolHandler;
            _playerButtonspoolHandler = lobbyPlayerButtonPoolHandler;
        }


        public void DisplayLobby(Lobby lobby)
        {   
            _playerButtonspoolHandler.Clear();
            foreach (var player in lobby.Players)
            {
                Debug.Log(player.Id);
                Debug.Log(player.Data == null);
                Debug.Log(player.Data[LobbyManager.Player_Name_Key].Value);
                Debug.Log(player.Data[LobbyManager.Player_Host_Key].Value);

                var @params = new LobbyPlayerButtonParams()
                {
                    playerName = player.Data[LobbyManager.Player_Name_Key].Value,
                    playerId = player.Id,
                };

                _playerButtonspoolHandler.Add(@params);
            }
        }

        public void DisplaySeeker(IEnumerable<Lobby> lobbies)
        {
            _poolHandler.Clear();
            foreach (var lobby in lobbies)
            {
                _poolHandler.Add(lobby.Id, lobby.Name);
            }
        }

        public void BlockStartButton(float dur)
        {
            StopCoroutine(nameof(Blocker));
            StartCoroutine(Blocker(dur));
        }

        private IEnumerator Blocker(float dur)
        {
            _startButton.interactable = false;
            for (float i = dur; i >= 0; i -= .5f)
            {
                yield return new WaitForSeconds(.5f);
                _startButtonText.text = i.ToString();
            }
            _startButtonText.text = "Start Game";
            _startButton.interactable = true;
        }

        public void UpdateState(LobbyManagerState state)
        {
            switch (state)
            {
                case LobbyManagerState.Seeker:
                    _seekerCanvas.SetActive(true);
                    _lobbyCanvas.SetActive(false);
                    break;
                case LobbyManagerState.Lobby:
                    _lobbyCanvas.SetActive(true);
                    _seekerCanvas.SetActive(false);
                    break;
            }
        }
    }
}