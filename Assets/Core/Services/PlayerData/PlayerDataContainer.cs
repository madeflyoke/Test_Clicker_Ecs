using System;
using Core.Services.PlayerData.Business;
using Core.Services.PlayerData.Currency;
using Newtonsoft.Json;

namespace Core.Services.PlayerData
{
    [Serializable]
    public class PlayerDataContainer
    {
        public BusinessModel BusinessModel;
        public MoneyCurrencyModel MoneyCurrencyModel;
        
        [JsonConstructor]
        public PlayerDataContainer(BusinessModel businessModel, MoneyCurrencyModel moneyCurrencyModel)
        {
            BusinessModel = businessModel;
            MoneyCurrencyModel = moneyCurrencyModel;
        }
    }
}
