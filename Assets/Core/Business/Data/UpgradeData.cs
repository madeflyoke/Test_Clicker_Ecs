using System;
using Core.Business.Enums;

namespace Core.Business.Data
{
    [Serializable]
    public struct UpgradeData
    {
        public UpgradeType UpgradeType;
        public double Price;
        public int IncomeMultiplier;
    }
}
