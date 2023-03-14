using System.Collections.Generic;
using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlateformSurvivor.Menu
{
    public class ShopService : MonoBehaviour
    {
        [SerializeField] private Button refundButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private ToggleGroup buyToggleGroup;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private PersistentDataManager persistentDataManager;
        
        public List<UpgradeObject> upgradeObjects;
        
        private void Start()
        {
            coinText.text = persistentDataManager.coins.ToString();
        }
        private void Update()
        {
            coinText.text = persistentDataManager.coins.ToString();
        }

        private bool CanBuy()
        {
            string upgradeSelected = buyToggleGroup.GetFirstActiveToggle().name;
            int currentLevel = persistentDataManager.statsUpgrade[upgradeSelected];
            int maxLevel = upgradeObjects.Find(upgradeObject => upgradeObject.upgradeName == upgradeSelected).maxLevel;
            return currentLevel < maxLevel ;
        }

        private void BuyPassiveUpgrade(string upgradeName)
        {
            persistentDataManager.statsUpgrade[upgradeName]++;
            refundButton.interactable = true;
            string upgradeSelected = buyToggleGroup.GetFirstActiveToggle().name;
            rankText.text = persistentDataManager.statsUpgrade[upgradeSelected] + "/" + upgradeObjects.Find(upgradeObject => upgradeObject.upgradeName == upgradeSelected).maxLevel;
            persistentDataManager.coins -= upgradeObjects.Find(upgradeObject => upgradeObject.upgradeName == upgradeSelected).basePrice;
            if (!CanBuy())
            {
                buyButton.interactable = false;
            }
        }
        
        public void ActiveBuy()
        {
            buyButton.onClick.RemoveAllListeners();
            if (buyToggleGroup.AnyTogglesOn() && CanBuy())
            {
                string upgradeSelected = buyToggleGroup.GetFirstActiveToggle().name;
                UpgradeObject upgradeObject = upgradeObjects.Find(upgradeObject => upgradeObject.upgradeName == upgradeSelected);
                priceText.text = upgradeObject.basePrice.ToString();
                rankText.text = persistentDataManager.statsUpgrade[upgradeSelected] + "/" + upgradeObject.maxLevel;
                buyButton.onClick.AddListener(delegate { BuyPassiveUpgrade(buyToggleGroup.GetFirstActiveToggle().name); });
                buyButton.interactable = true;
            }
            else
            {
                rankText.text = "";
                priceText.text = "";
                buyButton.interactable = false;
            }
        }

        public void Refund()
        {
            foreach (UpgradeObject upgradeObject in upgradeObjects)
            {
                for (int i=0; i< persistentDataManager.statsUpgrade[upgradeObject.upgradeName]; i++)
                {
                    persistentDataManager.coins += upgradeObject.basePrice;
                }
                persistentDataManager.statsUpgrade[upgradeObject.upgradeName] = 0;
            }
            buyToggleGroup.SetAllTogglesOff();
            refundButton.interactable = false;
        }
    }
}
