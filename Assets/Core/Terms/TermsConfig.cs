using System.Collections.Generic;
using Core.Business.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Terms
{
    [CreateAssetMenu(fileName = "TermsConfig", menuName = "Game/TermsConfig")]
    public class TermsConfig : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<BusinessType, string> _businessNamesData;
        [SerializeField] private Dictionary<UpgradeType, string> _upgradesNamesData;

        public string GetBusinessName(BusinessType businessType)
        {
            return _businessNamesData[businessType];
        }

        public string GetUpgradeName(UpgradeType upgradeType)
        {
            return _upgradesNamesData[upgradeType];
        }
    }
}
