using System.Linq;
using Core.Business.Components;
using Core.Business.Data;
using Core.Business.Enums;
using Core.Business.Upgrades.Components;
using Core.Common.Components;
using Core.Currency.Components;
using Core.Factory.Interfaces;
using Core.Services;
using Core.Services.PlayerData;
using Core.Terms;
using Core.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Factory
{
    public class BusinessEntityFactory : IEntityFactory
    {
        private EcsWorld _world;
        private BusinessConfig _dataConfig;
        private TermsConfig _termsConfig;
        private PlayerDataService _playerDataService;
        private ServicesProvider _servicesProvider;
        
        public void Initialize(EcsWorld world, ServicesProvider servicesProvider)
        {
            _world = world;
            _servicesProvider = servicesProvider;
            _dataConfig = _servicesProvider.GameDataProviderService.BusinessConfig;
            _termsConfig = _servicesProvider.GameDataProviderService.TermsConfig;
            _playerDataService = _servicesProvider.PlayerDataService;
        }

        public int Construct(BusinessType businessType) //todo divide by methods
        {
            var playerBusinessData = _playerDataService.BusinessMediator;
            
            var configData = _dataConfig.GetBusinessData(businessType); //todo blueprints
            var title = _termsConfig.GetBusinessName(businessType);
            
            var entity = _world.NewEntity();
            
            var level = playerBusinessData.HasBusiness(businessType) ? playerBusinessData.GetLevel(businessType) :
                (configData.PreOpened? 1:0);

            var businessBought = level > 0;
            
            if (businessBought)
            {
                AddPoolComponent<ActiveStateComponent>(entity);
            }
            
            AddPoolComponent<LevelComponent>(entity).Value = level;
            ref var levelUpPriceComponent = ref AddPoolComponent<LevelUpPriceComponent>(entity);
            levelUpPriceComponent.Value=FormulasUtils.CalculateNextLevelPrice(level, configData.BasePrice);
            levelUpPriceComponent.BaseValue = configData.BasePrice;
            
            AddPoolComponent<TitleComponent>(entity).Value = title;
            AddPoolComponent<BusinessTypeComponent>(entity).Value = businessType;
            
            ref var incomeMultiplierComponent = ref AddPoolComponent<IncomeMultiplierComponent>(entity);

            foreach (var upgradeType in playerBusinessData.GetUpgrades(businessType))
            {
                incomeMultiplierComponent.MultiplierPercent += configData.TryGetUpgrade(upgradeType).IncomeMultiplier;
            }
            
            ref var incomeComponent = ref AddPoolComponent<IncomeComponent>(entity);
            incomeComponent.Capacity = FormulasUtils.CalculateIncome(level, configData.BaseIncome,
                incomeMultiplierComponent.MultiplierPercent);
            incomeComponent.IncomeDuration = configData.IncomeDuration;
            incomeComponent.BaseIncome = configData.BaseIncome;
            incomeComponent.CurrentIncomeNormalized = businessBought? playerBusinessData.GetNormalizedIncomeProgress(businessType) :0;
            
            ref var progressBarComponent = ref AddPoolComponent<MoneyCurrencyProgressBarComponent>(entity);
            progressBarComponent.MaxValue = 1f;

            CreateUpgradeEntities(businessType, businessBought);
            
            return entity;
        }

        private void CreateUpgradeEntities(BusinessType businessType, bool isActive)
        {
            var configData = _dataConfig.GetBusinessData(businessType);

            foreach (var upgradeData in configData.Upgrades)
            {
                var entity = _world.NewEntity();
                
                var title = _termsConfig.GetUpgradeName(upgradeData.UpgradeType);
                
                AddPoolComponent<TitleComponent>(entity).Value = title;
                
                ref var upgradeComponent = ref AddPoolComponent<UpgradeComponent>(entity);
                upgradeComponent.Price = upgradeData.Price;
                upgradeComponent.MultiplierPercent = upgradeData.IncomeMultiplier;

                AddPoolComponent<BusinessTypeComponent>(entity).Value = businessType;
                AddPoolComponent<UpgradeTypeComponent>(entity).Value = upgradeData.UpgradeType;

                if (isActive)
                {
                    AddPoolComponent<ActiveStateComponent>(entity);
                }
            }
        }

        private ref T AddPoolComponent<T>(int entity) where T : struct
        {
            var pool = _world.GetPool<T>();
            return ref pool.Add(entity);
        }
    }
}