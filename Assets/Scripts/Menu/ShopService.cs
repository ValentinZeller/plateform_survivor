using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopService : MonoBehaviour
{
    public List<UpgradeObject> upgradeObjects;

    [SerializeField] Button refundButton;
    [SerializeField] Button buyButton;
    [SerializeField] ToggleGroup buyToggleGroup;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] PersistentDataManager persistentDataManager;


    private void Start()
    {
        coinText.text = persistentDataManager.coins.ToString();
    }
    private void Update()
    {
        coinText.text = persistentDataManager.coins.ToString();
    }

    public void ActiveBuy()
    {
        buyButton.onClick.RemoveAllListeners();
        if (buyToggleGroup.AnyTogglesOn() && CanBuy())
        {
            string upgradeSelected = buyToggleGroup.GetFirstActiveToggle().name;
            UpgradeObject upgradeObject = upgradeObjects.Find(upgradeObject => upgradeObject.upgradeName == upgradeSelected);
            priceText.text = upgradeObject.basePrice.ToString();
            buyButton.onClick.AddListener(delegate { BuyPassiveUpgrade(buyToggleGroup.GetFirstActiveToggle().name); });
            buyButton.interactable = true;
        }
        else
        {
            priceText.text = "";
            buyButton.interactable = false;
        }
    }

    private bool CanBuy()
    {
        string upgradeSelected = buyToggleGroup.GetFirstActiveToggle().name;
        int currentLevel = persistentDataManager.statsUpgrade[upgradeSelected];
        int maxLevel = upgradeObjects.Find(upgradeObject => upgradeObject.upgradeName == upgradeSelected).maxLevel;
        return maxLevel > currentLevel ;
    }

    public void BuyPassiveUpgrade(string upgradeName)
    {
        if (CanBuy()) { 
            persistentDataManager.statsUpgrade[upgradeName]++;
            refundButton.interactable = true;
            string upgradeSelected = buyToggleGroup.GetFirstActiveToggle().name;
            persistentDataManager.coins -= upgradeObjects.Find(upgradeObject => upgradeObject.upgradeName == upgradeSelected).basePrice;
        } else
        {
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
        refundButton.interactable = false;
    }
}
