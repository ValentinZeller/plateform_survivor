using PlateformSurvivor.Service;
using UnityEngine;

namespace PlateformSurvivor.Player
{
    public class PlayerLevel : MonoBehaviour
    {
        private float xp;
        private float xpNeeded = 2f;
        private float lvl = 1;
        private PlayerStat playerStat;
        private void Start()
        {
            playerStat = GetComponent<PlayerStat>();
            EventManager.AddListener("got_xp", LevelUp);
        }

        private void LevelUp(object data)
        {
            xp += (float)data;
            xp += xp * playerStat.currentStats["XpRate"];
            if (xp >= xpNeeded)
            {
                xp -= xpNeeded;
                xpNeeded += 1f;
                lvl++;
                EventManager.Trigger("level_up");
            }
            EventManager.Trigger("update_xp");
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
