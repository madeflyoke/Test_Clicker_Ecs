using System.Collections.Generic;
using Core.Business.Enums;
using Core.Services.PlayerData.Business;
using Core.Services.PlayerData.Currency;
using Core.Utils.Enums;
using Core.Utils.PlayerData;

namespace Core.Services.PlayerData
{
    public class PlayerDataService
    {
        private const string PlayerDataKey = "PlayerData";
        
        public BusinessModelMediator BusinessMediator { get; private set; }
        public MoneyCurrencyModelMediator MoneyCurrencyMediator { get; private set; }

        private readonly PlayerDataContainer _playerDataContainer;
        
        public PlayerDataService()
        {
            _playerDataContainer = JsonSaver.Load<PlayerDataContainer>(PlayerDataKey);

            if (_playerDataContainer == null)
            {
                _playerDataContainer = new PlayerDataContainer(
                    new BusinessModel(new Dictionary<BusinessType, BusinessModelData>()),
                    new MoneyCurrencyModel(0d));
                Save();
            }

            MoneyCurrencyMediator = new MoneyCurrencyModelMediator(_playerDataContainer.MoneyCurrencyModel);
            BusinessMediator = new BusinessModelMediator(_playerDataContainer.BusinessModel);
        }

        public void DebugDo()
        {
            MoneyCurrencyMediator.Operate(22, OperationType.Add);
            
            BusinessMediator.AddNewBusiness(BusinessType.BUSINESS4);
            BusinessMediator.AddLevel(BusinessType.BUSINESS4,4);
            BusinessMediator.AddUpgrade(BusinessType.BUSINESS4, UpgradeType.UPGRADE2);
            Save();
        }
        
        public void Save()
        {
            JsonSaver.Save(_playerDataContainer, PlayerDataKey);
        }
    }
}
