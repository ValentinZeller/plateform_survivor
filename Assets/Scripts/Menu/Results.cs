using PlateformSurvivor.Player;
using UnityEngine;
using TMPro;

namespace PlateformSurvivor.Menu
{
    public class Results : MonoBehaviour
    {
        private PersistentDataManager persistentDataManager;
        
        [SerializeField] private PlayerStat playerStat;
        [SerializeField] private PlayerLevel playerLevel;
        
        [SerializeField] private TextMeshProUGUI stage;
        [SerializeField] private TextMeshProUGUI character;
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private TextMeshProUGUI coin;
        [SerializeField] private TextMeshProUGUI lvl;
        [SerializeField] private TextMeshProUGUI kill;

        private void Awake()
        {
            if (FindObjectOfType<PersistentDataManager>())
            {
                persistentDataManager = FindObjectOfType<PersistentDataManager>();
            }
        }
        private void Start()
        {
            stage.text = persistentDataManager.chosenStage.displayName.GetLocalizedString();
            character.text = persistentDataManager.chosenCharacter.displayName;
            time.text = PlayerHUD.DisplayTime(playerStat.GetTime());
            coin.text = playerStat.GetCoins().ToString();
            lvl.text = playerLevel.GetLvl().ToString();
            kill.text = playerStat.GetKills().ToString();
        }
    }
}
