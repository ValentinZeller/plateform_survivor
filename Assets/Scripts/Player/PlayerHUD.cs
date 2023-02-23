using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerHP;
    [SerializeField] PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerHP.text = stat.currentStats["Health"].ToString();
    }
}
