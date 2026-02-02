using System.Collections.Generic;
using SlimeGround.Menu.Extensions.Windows;
using UnityEngine;

namespace UI.TabSystem
{
    public class TabSwitcher : MonoBehaviour
    {
        [SerializeField] private MenuWindow _parentWindow;
        [SerializeField] private List<TabData> _tabs;

        private TabData _activeTab;

        private void Awake()
        {
            foreach (TabData tabData in _tabs)
            {
                DeactivateTab(tabData);
            }

            _activeTab = _tabs[0];
        }

        private void OnEnable()
        {
            _parentWindow.MenuOpened += UpdateActiveTab;

            foreach (TabData tab in _tabs)
            {
                tab.TabButton.TabSelected += ActivateTab;
            }
        }

        private void OnDisable()
        {
            _parentWindow.MenuOpened -= UpdateActiveTab;
            
            foreach (TabData tab in _tabs)
            {
                tab.TabButton.TabSelected -= ActivateTab;
            }
        }

        public void UpdateActiveTab()
        {
            ActivateTab(_activeTab);
        }

        public void ActivateTab(int index)
        {
            ActivateTab(_tabs[index].TabButton);
        }

        private void ActivateTab(TabButton tabButton)
        {
            DeactivateTab(_activeTab);
            _activeTab = _tabs.Find(tab => tab.TabButton == tabButton);
            ActivateTab(_activeTab);
        }

        private void DeactivateTab(TabData tab)
        {
            if (tab == null)
            {
                return;
            }

            tab.TabContent.Deactivte();
            tab.TabButton.SetInactive();
        }

        private void ActivateTab(TabData tab)
        {
            if (tab == null)
            {
                return;
            }

            tab.TabContent.Activate();
            tab.TabButton.SetActive();
        }
    }
}