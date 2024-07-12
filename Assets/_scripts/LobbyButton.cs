
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BonBon
{


    public class LobbyButton : MonoBehaviour
    {
        private TMP_Text _text;
        private Button _button;
        private string _lobbyId;
        private ILobbyManager _lobbyManager;

        [Inject]
        public void Inject(ILobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
        }

        private void Awake()
        {
            Debug.Log("LobbyButtonSpawned");

            _text = GetComponentInChildren<TMP_Text>();
            _button = GetComponent<Button>();

            _button.onClick.AddListener(async () => await _lobbyManager.JoinLobby(_lobbyId));
        }
        public void SetUp(string lobbyId, string lobbyName)
        {
            if (lobbyId != _lobbyId)
            {
                _lobbyId = lobbyId;
                gameObject.name = lobbyName;
                _text.text = lobbyName;
            }

            gameObject.SetActive(true);
        }

        public void Clear()
        {
            gameObject.name = "Free lobby button";
            gameObject.SetActive(false);
        }
    }
}