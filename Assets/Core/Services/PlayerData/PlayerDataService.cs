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

        private PlayerDataContainer _playerDataContainer;
        
        public PlayerDataService(ServicesProvider servicesProvider)
        {
            _playerDataContainer = JsonSaver.Load<PlayerDataContainer>(PlayerDataKey);

            var newPlayer = false;
            if (_playerDataContainer == null)
            {
                HandleNewPlayer();
                newPlayer = true;
            }

            MoneyCurrencyMediator = new MoneyCurrencyModelMediator(_playerDataContainer.MoneyCurrencyModel);
            BusinessMediator = new BusinessModelMediator(_playerDataContainer.BusinessModel, 
                servicesProvider.GameDataProviderService.BusinessConfig,newPlayer);
        }

        private void HandleNewPlayer()
        {
            _playerDataContainer = new PlayerDataContainer(
                new BusinessModel(new Dictionary<BusinessType, BusinessModelData>()),
                new MoneyCurrencyModel(0d));
            Save();
        }
        
        public void Save()
        {
            JsonSaver.Save(_playerDataContainer, PlayerDataKey);
        }
    }
}
