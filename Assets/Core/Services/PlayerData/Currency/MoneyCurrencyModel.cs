using System;
using Newtonsoft.Json;

namespace Core.Services.PlayerData.Currency
{
    [Serializable]
    public class MoneyCurrencyModel
    {
        public double Value;

        [JsonConstructor]
        public MoneyCurrencyModel(double value)
        {
            Value = value;
        }
    }
}
