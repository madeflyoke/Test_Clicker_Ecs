using Core.Utils;
using TMPro;
using UnityEngine;

namespace Core.UI.Gameplay.Business
{
    public class IncomeInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _incomeText;

        public void SetIncomeValue(double value)
        {
            _incomeText.text = value.ToIntFloorString().WithDollarPostfix();
        }
    }
}
