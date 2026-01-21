using SlimeGround.Menu.Extensions.TabSystem;
using UnityEngine;

namespace UI.TabSystem
{
    [System.Serializable]
    public class TabData
    {
        [SerializeField] private TabButton _tabButton;
        [SerializeField] private TabContent _tabContent;

        public TabButton TabButton => _tabButton;
        public TabContent TabContent => _tabContent;
    }
}