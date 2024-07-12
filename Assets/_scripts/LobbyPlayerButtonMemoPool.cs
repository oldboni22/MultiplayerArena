using BonBon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LobbyPlayerButtonMemoPool : MonoMemoryPool<LobbyPlayerButtonParams,LobbyPlayerButton>
{
    private readonly List<LobbyPlayerButton> _buttons = new List<LobbyPlayerButton>(5);

    protected override void OnCreated(LobbyPlayerButton item)
    {
        _buttons.Add(item);
    }
    protected override void OnDespawned(LobbyPlayerButton item)
    {
        item.Clear();
    }
    protected override void Reinitialize(LobbyPlayerButtonParams @params, LobbyPlayerButton item)
    {
        item.SetUp(@params);
        base.Reinitialize(@params, item);
    }

    public void DespawnAll()
    {
        foreach (var item in _buttons)
        {
            if (item.gameObject.activeInHierarchy)
                Despawn(item);
        }
    }
}
