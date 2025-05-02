using System;
using System.Text;
using Core.Common.Interfaces;
using Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Gameplay.Business
{
    public class UpgradeView : MonoBehaviour, ICommonNotifyListener //decomposite by components if runtime change value
    {
        [field: SerializeField] public TMP_Text NameText { get; private set; }
        [field: SerializeField] public TMP_Text IncomeValueText { get; private set; }
        [field: SerializeField] public TMP_Text PriceText { get; private set; }
        [field: SerializeField] public Button UpgradeButton { get; private set; }
        
        [SerializeField] private GameObject _pricePartObj;
        [SerializeField] public GameObject _boughtTitleObj;
        private Action _onClick;

        public void SetupClickAction(Action clickAction)
        {
            UpgradeButton.interactable = true;
            _onClick = clickAction;
            UpgradeButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _onClick?.Invoke();
        }
        
        public void SetTitle(string title)
        {
            NameText.text = title;
        }

        public void SetPriceText(double value)
        {
            PriceText.text = value.ToIntFloorString().WithDollarPostfix();
        }

        public void SetIncomeValueText(int value)
        {
            IncomeValueText.text = value.ToString().ToPlusPercentView();
        }

        public void OnNotifyTriggered()
        {
            UpgradeButton.interactable = false;
            _boughtTitleObj.SetActive(true);
            _pricePartObj.SetActive(false);
        }
    }
}