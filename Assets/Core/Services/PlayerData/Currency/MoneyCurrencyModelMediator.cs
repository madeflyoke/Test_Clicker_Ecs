using System;
using Core.Utils.Enums;

namespace Core.Services.PlayerData.Currency
{
    public class MoneyCurrencyModelMediator
    {
        private readonly MoneyCurrencyModel _moneyModel;

        public MoneyCurrencyModelMediator(MoneyCurrencyModel moneyModel)
        {
            _moneyModel = moneyModel;
        }
        
        public void Operate(double value, OperationType operationType) // checked/unchecked?
        {
            var result = GetValue();
            switch (operationType)
            {
                case OperationType.Add:
                    result +=value; 
                    break;
                case OperationType.Subtract:
                    result -=value; 
                    break;
                case OperationType.Set:
                    result = value;
                    break;
            }

            result = Math.Clamp(result, 0d, double.MaxValue);
            _moneyModel.Value = result;
        }

        public double GetValue()
        {
            return _moneyModel.Value;
        }
    }
}
