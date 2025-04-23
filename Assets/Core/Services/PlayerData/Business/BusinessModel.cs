using System;
using System.Collections.Generic;
using Core.Business.Enums;
using Newtonsoft.Json;

namespace Core.Services.PlayerData.Business
{
    [Serializable]
    public class BusinessModel
    {
        public Dictionary<BusinessType, BusinessModelData> Data;
        
        [JsonConstructor]
        public BusinessModel(Dictionary<BusinessType, BusinessModelData> data)
        {
            Data = data;
        }

        public BusinessModelData GetData(BusinessType businessType)
        {
            Data.TryGetValue(businessType, out var result);
            return result;
        }
        
        public void AddData(BusinessType businessType, BusinessModelData businessModelData)
        {
            Data.TryAdd(businessType, businessModelData);
        }
    }
}
