using System;
using System.Collections.Generic;
using System.Linq;
using Core.Business.Data;
using Core.Business.Enums;

namespace Core.Services.PlayerData.Business
{
    public class BusinessModelMediator
    {
        public event Action OnSaveRequest;
        
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
            OnSaveRequest?.Invoke();
        }
        
        public void AddLevel(BusinessType businessType)
        {
            _businessModel.GetData(businessType).Level ++;
            OnSaveRequest?.Invoke();
        }

        public void AddUpgrade(BusinessType businessType, UpgradeType upgradeType)
        {
            if (HasBusiness(businessType)==false)
            {
                return;
            }
            
            var upgrades = _businessModel.GetData(businessType).BoughtUpgrades;
            if (upgrades.Contains(upgradeType)==false)
            {
                upgrades.Add(upgradeType);
                OnSaveRequest?.Invoke();
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
            return _businessModel.GetData(businessType)==null? new List<UpgradeType>():
                _businessModel.GetData(businessType).BoughtUpgrades;
        }
        
        private void HandeNewPlayer()
        {
            foreach (var kvp in
                     _businessConfig.GetAllBusinessData().Where(x=>x.Value.PreOpened))
            {
                AddNewBusiness(kvp.Key);
            }
            OnSaveRequest?.Invoke();
        }
    }
}
