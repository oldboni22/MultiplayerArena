
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Zenject;

namespace BonBon
{


    public class LobbySceneInstaller : MonoInstaller
    {
        [Header("Prefabs")]
        [SerializeField] private LobbyButton _lobbyButtonPrefab;
        [SerializeField] private LobbyPlayerButton _lobbyPlayerButtonPrefab;

        [Header("Objects")]
        [SerializeField] private LobbyManager _lobbyManager;
        [SerializeField] private LobbyMemoPoolHandler _lobbyButtonMemoPool;
        [SerializeField] private LobbyPlayerButtonPoolHandler _lobbyPlayerButtonMemoPool;
        [SerializeField] private LobbiesUiManager _lobbyUiManager;

        public override void InstallBindings()
        {


            Container.BindMemoryPool<LobbyButton, LobbyButtonMemoPool>().WithInitialSize(5).FromComponentInNewPrefab(_lobbyButtonPrefab).NonLazy();
            Container.BindMemoryPool<LobbyPlayerButton, LobbyPlayerButtonMemoPool>().WithInitialSize(5).FromComponentInNewPrefab(_lobbyPlayerButtonPrefab).NonLazy();


            Container.Bind<ILobbiesUiManager>().To<LobbiesUiManager>().FromInstance(_lobbyUiManager).AsCached().NonLazy();

            Container.Bind<ILobbyPlayerButtonPoolHandler>().To<LobbyPlayerButtonPoolHandler>().FromInstance(_lobbyPlayerButtonMemoPool).AsCached().NonLazy();
            Container.Bind<ILobbyMemoPoolHandler>().To<LobbyMemoPoolHandler>().FromInstance(_lobbyButtonMemoPool).AsCached().NonLazy();
            Container.Bind<ILobbyManager>().To<LobbyManager>().FromInstance(_lobbyManager).AsCached().NonLazy();


        }
    }
}