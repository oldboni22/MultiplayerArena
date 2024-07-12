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
using Zenject;

namespace BonBon
{
    public enum LobbyManagerState
    {
        Seeker,
        Lobby
    }
    public interface ILobbyManager
    {
        public Task JoinLobby(string lobbyId);
        public Task KickFromLobby(string playerId);
        public bool IsHost();
    }
    public class LobbyManager : NetworkBehaviour, ILobbyManager
    {
        private ILobbiesUiManager _lobbiesUiManager;

        private LobbyManagerState _state;
        private async Task SetState(LobbyManagerState state)
        {
            _state = state;
            _lobbiesUiManager.UpdateState(_state);
            await UpdateState();
        }

        private bool _isHost = false;
        public bool IsHost() => _isHost;


        public readonly static string Join_Code_Key = "joinCode";
        public readonly static string Player_Name_Key = "playerName";
        public readonly static string Player_Host_Key = "IsHost";

        [SerializeField] private Button _updateButton;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _leaveButton;


        private float _lobbyHeartBeatRate;
        private float _lobbyUpdateRate = 3.5f;
        private Lobby _hostedLobby;
        private Lobby _joinedLobby;

        [Inject]
        public void Inject(ILobbiesUiManager lobbiesUiManager)
        {
            _lobbiesUiManager = lobbiesUiManager;
        }

        private async void FixedUpdate()
        {
            await HandleHeartBeat();
            await HandleUpdateTimer();
        }

        async void Start()
        {
            Debug.Log("START");
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            _state = LobbyManagerState.Seeker;


            _updateButton.onClick.AddListener(async () => await UpdateState());

            _addButton.onClick.AddListener(async () => await CreateLobby());

            _leaveButton.onClick.AddListener(async () => await LeaveLobby());

            _startButton.onClick.AddListener(StartGame);
        }

        public void StartGame()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
        }

        #region Lobby
        private async Task HandleHeartBeat()
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

        private async Task HandleUpdateTimer()
        {
            _lobbyUpdateRate -= Time.fixedDeltaTime;
            if (_lobbyUpdateRate <= 0)
            {
                _lobbyUpdateRate = 1.25f;
                await UpdateState();
                Debug.Log("StateUpdate");
            }
        }

        private async Task CreateLobby()
        {
            try
            {
                string joinCode = await CreateRelay();
                var playerName = PlayerPrefsManager.GetLocalPlayerData()._name;

                CreateLobbyOptions options = new CreateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        { Join_Code_Key, new DataObject(DataObject.VisibilityOptions.Member, joinCode) },
                        { Player_Name_Key, new DataObject(DataObject.VisibilityOptions.Public, playerName) },
                    }

                };


                var lobby = await LobbyService.Instance.CreateLobbyAsync($"{playerName}'s lobby", 5, options);

                Debug.Log(lobby.Data[Join_Code_Key].Value);



                _hostedLobby = lobby;
                _joinedLobby = lobby;

                await LobbyService.Instance.UpdatePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
                {
                    Data = new Dictionary<string, PlayerDataObject>
                {
                    { Player_Name_Key, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) },
                    { Player_Host_Key, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "0") }
                }
                });

                await SetState(LobbyManagerState.Lobby);
                AddLobbyEvents();
                _isHost = true;
                _startButton.gameObject.SetActive(true);

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        private async Task UpdateState()
        {
            try
            {
                switch (_state)
                {
                    case LobbyManagerState.Seeker:
                        var query = await Lobbies.Instance.QueryLobbiesAsync();
                        _lobbiesUiManager.DisplaySeeker(query.Results);
                        break;
                    case LobbyManagerState.Lobby:
                        _joinedLobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id);

                        Debug.Log(_joinedLobby.Players[0].Id);
                        Debug.Log(_joinedLobby.Players[0].Data == null);

                        _lobbiesUiManager.DisplayLobby(_joinedLobby);
                        break;

                }


            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }

        public async Task JoinLobby(string lobbyId)
        {
            try
            {

                string playerName = PlayerPrefsManager.GetLocalPlayerData()._name;
                JoinLobbyByIdOptions options = new JoinLobbyByIdOptions()
                {
                    Player = new Unity.Services.Lobbies.Models.Player()
                    {
                        Data = new Dictionary<string, PlayerDataObject>()
                        {
                            { Player_Name_Key, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) },
                            { Player_Host_Key, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "1") }
                        }
                    }
                };

                var joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, options);
                Debug.Log("Joined Lobby:" + joinedLobby.Name);

                var join = await JoinRelay(joinedLobby.Data[Join_Code_Key].Value);
                if (join is false)
                    await Lobbies.Instance.RemovePlayerAsync(lobbyId, AuthenticationService.Instance.PlayerId);
                else
                {
                    _joinedLobby = joinedLobby;
                    await SetState(LobbyManagerState.Lobby);
                    Debug.Log(_joinedLobby.Players.Count);
                }

            }
            catch (Exception e)
            {

                await Lobbies.Instance.RemovePlayerAsync(lobbyId, AuthenticationService.Instance.PlayerId);
                Debug.Log("Failed to join Relay, kicked from lobby");
                Debug.Log(e.Message);
            }
        }

        private async void AddLobbyEvents()
        {
            if (_hostedLobby == null) return;

            var callbacks = new LobbyEventCallbacks();
            callbacks.PlayerJoined += (List<LobbyPlayerJoined> a) => { _lobbiesUiManager.BlockStartButton(5); };
            callbacks.PlayerJoined += (List<LobbyPlayerJoined> a) => { Debug.Log(_hostedLobby.Players.Count); Debug.Log(_joinedLobby.Players.Count); };


            await Lobbies.Instance.SubscribeToLobbyEventsAsync(_hostedLobby.Id, callbacks);
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

        public async Task LeaveLobby()
        {
            await Lobbies.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            _joinedLobby = null;
            _hostedLobby = null;
            NetworkManager.Singleton.Shutdown();
            await SetState(LobbyManagerState.Seeker);
        }
        public async Task KickFromLobby(string playerId)
        {

        }
        #endregion
    }

}