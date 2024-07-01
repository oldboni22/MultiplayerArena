using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerNickName : NetworkBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void SetName(string name)
    {
        _text.text = name;
    }
}
