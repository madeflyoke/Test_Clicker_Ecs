using Core.Business.Components;
using Core.Common.Interfaces;
using TMPro;
using UnityEngine;

namespace Core.UI.Gameplay.Business
{
    public class LevelInfoView : MonoBehaviour, IValueChangedListener<int>
    {
        [SerializeField] private TMP_Text _levelText;
        
        public void OnValueChanged(int value)
        {
            _levelText.text = value.ToString();
        }
    }
}
