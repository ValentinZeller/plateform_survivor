using UnityEngine;

namespace PlateformSurvivor.Save
{
    public static class GamePreferencesManager
    {

        public static void SavePrefs()
        {
            PlayerPrefs.Save();
        }

        public static void LoadPrefs()
        {
            
        }
    }
}