using TMPro;
using UnityEngine;

namespace Core.UI.Gameplay.Business
{
    public class UpgradeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _incomeValueText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private GameObject _boughtObj;
    }
}
