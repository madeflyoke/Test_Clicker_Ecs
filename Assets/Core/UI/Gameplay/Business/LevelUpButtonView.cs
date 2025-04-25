using System;
using Core.Common.Interfaces;
using Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Gameplay.Business
{
    public class LevelUpButtonView : MonoBehaviour, IValueChangedListener<double>
    {
        [SerializeField] private TMP_Text _levelPriceText;
        [SerializeField] private Button _button;
        private Action _onClick;
        
        public void SetupClickAction(Action onClick, double price)
        {
            _onClick = onClick;
        }
        
        public void OnValueChanged(double value)
        {
            _levelPriceText.text = value.ToIntFloorString().WithDollarPostfix();
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }
        
        private void OnClick()
        {
            _onClick?.Invoke();
        }
    }
}
