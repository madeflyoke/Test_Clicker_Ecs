using System.Collections.Generic;
using System.Linq;
using Core.Business.Data;
using Core.Business.Enums;

namespace Core.Services.PlayerData.Business
{
    public class BusinessModelMediator
    {
        private readonly BusinessModel _businessModel;
        private readonly BusinessConfig _businessConfig;

        public BusinessModelMediator(BusinessModel businessModel, BusinessConfig businessConfig, bool asNewPlayer)
        {
            _businessModel = businessModel;
            _businessConfig = businessConfig;
            if (asNewPlayer)
            {
                HandeNewPlayer();
            }
        }
        
        public void AddNewBusiness(BusinessType businessType)
        {
            _businessModel.AddData(businessType, new BusinessModelData(){Level = 1});
        }
        
        public void AddLevel(BusinessType businessType)
        {
            _businessModel.GetData(businessType).Level ++;
        }

        public void AddUpgrade(BusinessType businessType, UpgradeType upgradeType)
        {
            var upgrades = _businessModel.GetData(businessType).BoughtUpgrades;
            if (upgrades.Contains(upgradeType)==false)
            {
                upgrades.Add(upgradeType);
            }
        }

        public bool HasBusiness(BusinessType businessType)
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
        
        private void HandeNewPlayer()
        {
            foreach (var kvp in
                     _businessConfig.GetAllBusinessData().Where(x=>x.Value.PreOpened))
            {
                AddNewBusiness(kvp.Key);
            }
        }
    }
}
