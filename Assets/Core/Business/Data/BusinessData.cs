using System;
using System.Collections.Generic;

namespace Core.Business.Data
{
    [Serializable]
    public class BusinessData
    {
        public int IncomeDuration;
        public double BasePrice;
        public int BaseIncome;
        public List<UpgradeData> Upgrades;
        public bool PreOpened;
    }
}
