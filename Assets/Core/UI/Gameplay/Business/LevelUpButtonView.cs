using System;
using Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Gameplay.Business
{
    public class LevelUpButtonView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelPriceText;
        [SerializeField] private Button _button;
        private Action _onClick;
        
        public void Setup(Action onClick, double price)
        {
            _onClick = onClick;
            _levelPriceText.text = price.ToIntFloorString().WithDollarPostfix();
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
