using System;
using System.Collections.Generic;
using Core.Business.Enums;

namespace Core.Services.PlayerData.Business
{
    [Serializable]
    public class BusinessModelData
    {
        public int Level;
        public List<UpgradeType> BoughtUpgrades = new List<UpgradeType>();
        public float NormalizedIncomeProgress;
    }
}
