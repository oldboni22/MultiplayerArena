using BonBon;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerButton : NetworkBehaviour
{
    [SerializeField] private GameObject _hostBtn;
    [SerializeField] private GameObject _clientBtn;
    [SerializeField] private GameObject _startBtn;
    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("GamePlay",LoadSceneMode.Single);
    }
    public void Host()
    {
        
        NetworkManager.Singleton.StartHost();
        Destroy(_hostBtn);
        Destroy(_clientBtn);
        _startBtn.SetActive(true);
    }
    public void Client()
    {
        NetworkManager.Singleton.StartClient();
        Destroy(_hostBtn);
        Destroy(_clientBtn);
        Destroy(_startBtn);
    }
}
