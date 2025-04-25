using Core.Common.Interfaces;
using Core.Utils;
using TMPro;
using UnityEngine;

namespace Core.UI.Gameplay.Business
{
    public class IncomeInfoView : MonoBehaviour, IValueChangedListener<double>
    {
        [SerializeField] private TMP_Text _incomeText;
        
        public void OnValueChanged(double value)
        {
            _incomeText.text = value.ToIntFloorString().WithDollarPostfix();
        }
    }
}
