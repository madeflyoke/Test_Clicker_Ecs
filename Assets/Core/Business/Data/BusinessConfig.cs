using System.Collections.Generic;
using Core.Business.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Business.Data
{
    [CreateAssetMenu(fileName = "BusinessConfig", menuName = "Game/BusinessConfig")]
    public class BusinessConfig : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<BusinessType, BusinessData> _businessData;

        public BusinessData GetBusinessData(BusinessType businessType)
        {
            return _businessData[businessType];
        }
        
        public Dictionary<BusinessType, BusinessData> GetAllBusinessData()
        {
            return _businessData;
        }
    }
}
