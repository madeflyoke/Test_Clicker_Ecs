using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Gameplay.Business
{
    public class BusinessElementView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private Image _mainIcon;

        [field: SerializeField] public MoneyCurrencyProgressBar MoneyCurrencyProgressBar {get; private set;}
        [field: SerializeField] public LevelInfoView LevelInfoView {get; private set;}
        [field: SerializeField] public IncomeInfoView IncomeInfoView {get; private set;}
        [field: SerializeField] public LevelUpButtonView LevelUpButtonView { get; private set; }
        [field: SerializeField] public List<UpgradeView> UpgradeViews{ get; private set; }

        public void SetTitle(string title)
        {
            _titleText.text = title;
        }
        
        public void SetIcon(Sprite sprite)
        {
            _mainIcon.sprite = sprite;
        }
    }
}
