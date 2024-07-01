using BonBon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorPicker : MonoBehaviour
{
    [SerializeField] private string _colId;
    public void SetColor() => PlayerPrefsManager.SetColor(_colId);
}
