
using TMPro;
using UnityEngine;

namespace BonBon
{


    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _field;

        private void Start()
        {
            if (PlayerPrefsManager.HasName())
            {
                _field.text = PlayerPrefsManager.GetLocalPlayerData()._name;
            }
            _field.onEndEdit.AddListener(SetNewName);
    }



        private void SetNewName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                PlayerPrefsManager.RemoveName();
            else
                PlayerPrefsManager.SetName(newName);
        }
    }
}