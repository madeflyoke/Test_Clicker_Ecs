using System;
using Core.Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Gameplay
{
    public class MoneyCurrencyProgressBar : MonoBehaviour, IValueChangedListener<float>
    {
        [SerializeField] private Slider _slider;
        
        public void OnValueChanged(float value)
        {
            _slider.value = value;
        }
    }
}
