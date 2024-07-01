using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BonBon
{

    public class PlayerData
    {
        public readonly string _name,_colId;
        public PlayerData(string name, string colId)
        {
            _name = name;
            _colId = colId;
        }
    }

    public static class PlayerPrefsManager
    {

        private static readonly string COLOR_KEY = "localColor";
        private static readonly string NAME_KEY = "localName";
        public static PlayerData GetLocalPlayerData() 
        {
            string name = " ";
            string colId = " ";

            if (PlayerPrefs.HasKey(NAME_KEY) is false)
                name = "No name";
            else
            {
                name = PlayerPrefs.GetString(NAME_KEY);
            }

            if(PlayerPrefs.HasKey(COLOR_KEY) is false)
                colId = "blue";
            else
            {
                colId = PlayerPrefs.GetString(COLOR_KEY);
            }
                
            return new PlayerData(name, colId);
        }

        public static bool HasName() => PlayerPrefs.HasKey(NAME_KEY);

        public static void SetColor(string col)
        {
            PlayerPrefs.SetString(COLOR_KEY, col);
        }
        public static void SetName(string name)
        {
            PlayerPrefs.SetString (NAME_KEY, name);
        }

        public static void RemoveName()
        {
            PlayerPrefs.DeleteKey(NAME_KEY);
        }
        public static void SetLocalPlayerData(string name, string colId)
        {
            PlayerPrefs.SetString(COLOR_KEY, colId);
            PlayerPrefs.SetString(NAME_KEY, name);

            PlayerPrefs.Save();

        }

    }
}