using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlateformSurvivor.Menu
{
    public class TabGroup : MonoBehaviour
    {
        public List<GameObject> objectsToSwap = new();
        public List<TabButton> tabButtons = new();
        public Color tabIdle;
        public Color tabHover;
        public Color tabActive;

        public TabButton selectedTab;

        private void Start()
        {
            ResetTabs();
            if (selectedTab != null)
            {
                selectedTab.background.color = tabActive;
            }
            
        }

        public void Subscribe(TabButton button)
        {
            tabButtons.Add(button);
        }
        
        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            if (selectedTab == null || button != selectedTab) {
                button.background.color = tabHover;
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab = button;
            ResetTabs();
            button.background.color = tabActive;
            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i < objectsToSwap.Count; i++)
            {
                if (i == index)
                {
                    objectsToSwap[i].SetActive(true);
                }
                else
                {
                    objectsToSwap[i].SetActive(false);
                }
            }
        }

        public void ResetTabs()
        {
            foreach (TabButton button in tabButtons)
            {
                if (selectedTab != null && button == selectedTab)
                {
                    continue;
                }
                button.background.color = tabIdle;
            }
        }
    }
}
