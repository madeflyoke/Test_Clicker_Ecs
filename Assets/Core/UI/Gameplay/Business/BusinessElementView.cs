using System.Collections.Generic;
using Core.UI.Common;
using TMPro;
using UnityEngine;

namespace Core.UI.Gameplay.Business
{
    public class BusinessElementView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private ProgressBar _progressBar;
        [SerializeField] private LevelInfoView _levelInfoView;
        [SerializeField] private IncomeInfoView _incomeInfoView;
        [SerializeField] private LevelUpButtonView _levelUpButtonView;
        [SerializeField] private List<UpgradeView> _upgradeViews;

    }
}
