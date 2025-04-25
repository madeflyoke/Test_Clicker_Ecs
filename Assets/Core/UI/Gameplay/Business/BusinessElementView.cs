using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core.UI.Gameplay.Business
{
    public class BusinessElementView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        
        [field: SerializeField] public MoneyCurrencyProgressBar MoneyCurrencyProgressBar {get; private set;}
        [field: SerializeField] public LevelInfoView LevelInfoView {get; private set;}
        [field: SerializeField] public IncomeInfoView IncomeInfoView {get; private set;}
        [field: SerializeField] public LevelUpButtonView LevelUpButtonView { get; private set; }
        [SerializeField] private List<UpgradeView> _upgradeViews;

        public void SetTitle(string title)
        {
            _titleText.text = title;
        }
    }
}
