using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BonBon
{


    public class LobbyManager : NetworkBehaviour
    {
        private string Join_Code_Key = "joinCode";

        [SerializeField] private Button _updateButton;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _startButton;

        //INCAPSULATE LATER 
        [SerializeField] private GameObject _lobbyButtonPref;
        [SerializeField] private Transform _buttonsTransform;


        private float _lobbyHeartBeatRate;
        private Lobby _hostedLobby;


        private void FixedUpdate()
        {
            HandleHeartBeat();
        }

        async void Start()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            _updateButton.onClick.AddListener(UpdateLobbies);

            _addButton.onClick.AddListener(CreateLobby);
            _addButton.onClick.AddListener(UpdateLobbies);

            _startButton.onClick.AddListener(StartGame);
        }

        public void StartGame()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
        }

        #region Lobby
        private async void HandleHeartBeat()
        {
            if (_hostedLobby == null) return;

            _lobbyHeartBeatRate -= Time.fixedDeltaTime;
            if (_lobbyHeartBeatRate <= 0)
            {
                Debug.Log(_hostedLobby.Name + "Heartbeat");
                _lobbyHeartBeatRate = 10;
                await LobbyService.Instance.SendHeartbeatPingAsync(_hostedLobby.Id);
            }
        }



        private async void CreateLobby()
        {
            try
            {
                string joinCode = await CreateRelay();

                CreateLobbyOptions options = new CreateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        { Join_Code_Key, new DataObject(DataObject.VisibilityOptions.Public,joinCode)}
                    }

                };

                var playerName = PlayerPrefsManager.GetLocalPlayerData()._name;
                var lobby = await LobbyService.Instance.CreateLobbyAsync($"{playerName}'s lobby", 5, options);
                Debug.Log(lobby.Data[Join_Code_Key].Value);
                _hostedLobby = lobby;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        private async void UpdateLobbies()
        {
            try
            {
                var query = await Lobbies.Instance.QueryLobbiesAsync();
                foreach (var lobby in query.Results)
                {
                    Debug.Log(lobby.Name);

                    //INCAPSULATE LATER
                    var buttonScript = GameObject.Instantiate(_lobbyButtonPref, _buttonsTransform).GetComponent<LobbyButton>();
                    buttonScript.SetUp(lobby, JoinLobby);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }

        private async void JoinLobby(Lobby lobby)
        {
            try
            {

                await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
                Debug.Log("Joined Lobby" + lobby.Name);

                var join = await JoinRelay(lobby.Data[Join_Code_Key].Value);
                if (join is false)
                    await Lobbies.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);

            }
            catch (Exception e)
            {

                await Lobbies.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);
                Debug.Log("Failed to join Relay, kicked from lobby");
                Debug.Log(e.Message);
            }
        }
        #endregion



        #region Relay
        async Task<string> CreateRelay()
        {
            try
            {
                var alloc = await RelayService.Instance.CreateAllocationAsync(4);

                var relayServData = new RelayServerData(alloc, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServData);

                //NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                //    alloc.RelayServer.IpV4,
                //    (ushort)alloc.RelayServer.Port,
                //    alloc.AllocationIdBytes,
                //    alloc.Key,
                //    alloc.AllocationIdBytes
                //    ) ;


                var code = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
                Debug.Log(NetworkManager.Singleton.StartHost());

                return code;

            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return null;
            }
        }
        private async Task<bool> JoinRelay(string code)
        {
            try
            {
                Debug.Log("Trying to join relay");
                var alloc = await RelayService.Instance.JoinAllocationAsync(code);

                var relayServData = new RelayServerData(alloc, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServData);


                //NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                //    alloc.RelayServer.IpV4,
                //    (ushort)alloc.RelayServer.Port,
                //    alloc.AllocationIdBytes,
                //    alloc.Key,
                //    alloc.AllocationIdBytes,
                //    alloc.HostConnectionData
                //    );

                var result = NetworkManager.Singleton.StartClient();
                Debug.Log(result);
                return result;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return false;
            }
        }
        #endregion
    }

}