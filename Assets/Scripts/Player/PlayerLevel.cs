using PlateformSurvivor.Service;
using UnityEngine;

namespace PlateformSurvivor.Player
{
    public class PlayerLevel : MonoBehaviour
    {
        private float xp;
        private float xpNeeded = 2f;
        private float lvl = 1;
        private void Start()
        {
            EventManager.AddListener("got_xp", LevelUp);
        }

        private void LevelUp(object data)
        {
            xp += (float)data;
            if (!(xp >= xpNeeded)) return;
            
            xp = 0;
            xpNeeded += 1f;
            lvl++;
            EventManager.Trigger("level_up");
        }

    }
}
