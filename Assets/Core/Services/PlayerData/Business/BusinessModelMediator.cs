using System.Collections.Generic;
using Core.Business.Enums;

namespace Core.Services.PlayerData.Business
{
    public class BusinessModelMediator
    {
        private readonly BusinessModel _businessModel;

        public BusinessModelMediator(BusinessModel businessModel)
        {
            _businessModel = businessModel;
        }
        
        public void AddNewBusiness(BusinessType businessType)
        {
            _businessModel.AddData(businessType, new BusinessModelData(){Level = 1});
        }
        
        public void AddLevel(BusinessType businessType, int appendValue)
        {
            _businessModel.GetData(businessType).Level += appendValue;
        }

        public void AddUpgrade(BusinessType businessType, UpgradeType upgradeType)
        {
            var upgrades = _businessModel.GetData(businessType).BoughtUpgrades;
            if (upgrades.Contains(upgradeType)==false)
            {
                upgrades.Add(upgradeType);
            }
        }

        public bool IsBusinessBought(BusinessType businessType)
        {
            return _businessModel.GetData(businessType)!= null;
        }
        
        public int GetLevel(BusinessType businessType)
        {
            var target = _businessModel.GetData(businessType);
            if (target == null)
            {
                return -1;
            }
            return target.Level;
        }

        public List<UpgradeType> GetUpgrades(BusinessType businessType)
        {
            return _businessModel.GetData(businessType)?.BoughtUpgrades;
        }
    }
}
