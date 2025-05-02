using System;
using System.Collections.Generic;
using System.Linq;
using Core.Business.Enums;
using UnityEngine;

namespace Core.Business.Data
{
    [Serializable]
    public class BusinessData
    {
        public Sprite Icon;
        public int IncomeDuration;
        public double BasePrice;
        public int BaseIncome;
        public List<UpgradeData> Upgrades;
        public bool PreOpened;

        public UpgradeData TryGetUpgrade(UpgradeType type)
        {
            return Upgrades.FirstOrDefault(x=>x.UpgradeType == type);
        }
    }
}
