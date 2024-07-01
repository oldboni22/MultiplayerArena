using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButton : MonoBehaviour
{
    private TMP_Text _text;
    private Button _button;

    public void SetUp(Lobby lobby, Action<Lobby> onClick)
    {
        _text = GetComponentInChildren<TMP_Text>();
        _button = GetComponent<Button>();


        gameObject.name = lobby.Name;
        _text.text = lobby.Name;
        _button.onClick.AddListener(() => onClick.Invoke(lobby) );
    }

}
