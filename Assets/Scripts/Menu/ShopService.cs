using System.Collections.Generic;
using ScriptableObject;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace PlateformSurvivor.Menu
{
    public class ShopService : MonoBehaviour
    {
        [SerializeField] private Button refundButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private GameObject togglePrefab;
        [SerializeField] private ToggleGroup buyToggleGroup;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private PersistentDataManager persistentDataManager;
        
        private List<UpgradeObject> upgradeObjects = new();
        
        private void Start()
        {
            coinText.text = persistentDataManager.coins.ToString();
            
            foreach (var upgrade in persistentDataManager.statsUpgrade)
            {
                GameObject instance = Instantiate(togglePrefab, buyToggleGroup.transform);
                instance.name = upgrade.Key;
                instance.GetComponent<Toggle>().group = buyToggleGroup;
                instance.GetComponent<Toggle>().onValueChanged.AddListener(delegate { ActiveBuy(); });
                UpgradeObject upgradeObject = Resources.Load<UpgradeObject>("CustomData/Upgrades/" + upgrade.Key);
                instance.GetComponentInChildren<Text>().text = upgradeObject.displayName.GetLocalizedString();
                upgradeObjects.Add(upgradeObject);
            }
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
            int maxPrice = upgradeObjects.Find(upgradeObject => upgradeObject.upgradeName == upgradeSelected).basePrice;
            return currentLevel < maxLevel && persistentDataManager.coins > maxPrice ;
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
            buyButton.interactable = false;
            rankText.text = "";
            priceText.text = "";
            
            if (buyToggleGroup.AnyTogglesOn())
            {
                string upgradeSelected = buyToggleGroup.GetFirstActiveToggle().name;
                UpgradeObject upgradeObject = upgradeObjects.Find(upgradeObject => upgradeObject.upgradeName == upgradeSelected);
                priceText.text = upgradeObject.basePrice.ToString();
                rankText.text = persistentDataManager.statsUpgrade[upgradeSelected] + "/" + upgradeObject.maxLevel;
                if (CanBuy())
                {
                    buyButton.onClick.AddListener(delegate { BuyPassiveUpgrade(buyToggleGroup.GetFirstActiveToggle().name); });
                    buyButton.interactable = true;
                }
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
