using BonBon;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class test : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    // Start is called before the first frame update
    private static test _instance;

    private IPlayerAnimatorsStorage _animatorsStorage;
    private IConstraintManager _constraintManager;

    [Inject]
    public void Init(IConstraintManager constraintManager, IPlayerAnimatorsStorage animatorsStorage)
    {
        _constraintManager = constraintManager;
        _animatorsStorage = animatorsStorage;
    }


    public override void OnNetworkSpawn()
    {       
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SpawnPlayers;
        base.OnNetworkSpawn();
    }

    private void Awake()
    {
        if (_instance != null) Destroy(gameObject);

        if (_instance == null)
        {
            DontDestroyOnLoad(this);
            _instance = this;
        }
    }

    private void SpawnPlayers(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (IsHost is false) return;
        if (sceneName != "GamePlay") return;

        try
        {
            foreach (var client in clientsCompleted)
            {
                Debug.Log(client.ToString());      
                SpawnPlayer(client);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void SpawnPlayer(ulong client)
    {
        var constraints = _constraintManager.GetDefault();

        var player = GameObject.Instantiate(_playerPrefab);
        var playerScript = player.GetComponent<Player>();

        
        playerScript.SetAnimatorsStorage(_animatorsStorage);
        playerScript.SetConstraints(constraints);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(client, true);
        
    }

}
