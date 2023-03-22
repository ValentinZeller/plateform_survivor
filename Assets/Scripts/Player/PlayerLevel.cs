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
            
            if (xp >= xpNeeded)
            {
                xp -= xpNeeded;
                xpNeeded += 1f;
                lvl++;
                EventManager.Trigger("level_up");
            }
        }

        public float GetXp()
        {
            return xp;
        }

        public float GetXpNeeded()
        {
            return xpNeeded;
        }

        public float GetLvl()
        {
            return lvl;
        }

    }
}
