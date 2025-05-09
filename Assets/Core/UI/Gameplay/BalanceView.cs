using Core.Common.Interfaces;
using Core.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Core.UI.Gameplay
{
    public class BalanceView : MonoBehaviour, IValueChangedListener<double>
    {
        [SerializeField] private TMP_Text _text;
        
        private void RefreshView(double value)
        {
            _text.text = value.ToIntFloorString().WithDollarPostfix();
        }

        public void OnValueChanged(double value)
        {
            RefreshView(value);
        }
    }
}
