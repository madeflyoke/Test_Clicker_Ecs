using Core.Business.Components;
using Core.Business.Data;
using Core.Business.Enums;
using Core.Business.Upgrades.Components;
using Core.Common.Components;
using Core.Currency.Components;
using Core.Factory.Interfaces;
using Core.Services;
using Core.Services.PlayerData;
using Core.Services.PlayerData.Business;
using Core.Terms;
using Core.Utils;
using Leopotam.EcsLite;

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

        public int Construct(BusinessType businessType)
        {
            var playerBusinessData = _playerDataService.BusinessMediator;
            var configData = _dataConfig.GetBusinessData(businessType);
            
            var entity = _world.NewEntity();
            var level = CreateLevelComponents(entity,businessType,configData, playerBusinessData);
            var businessBought = level > 0;
            
            if (businessBought)
            {
                AddPoolComponent<ActiveStateComponent>(entity);
            }
            
            var title = _termsConfig.GetBusinessName(businessType);
            AddPoolComponent<TitleComponent>(entity).Value = title;
            AddPoolComponent<BusinessTypeComponent>(entity).Value = businessType;
            
            CreateIncomeComponents(entity, level, configData, playerBusinessData, businessBought, businessType);
            
            AddPoolComponent<MoneyCurrencyProgressBarComponent>(entity).MaxValue = 1f;

            CreateUpgradeEntities(businessType, businessBought);
            
            return entity;
        }

        private int CreateLevelComponents(int entity, BusinessType businessType, BusinessData configData, BusinessModelMediator playerBusinessData)
        {
            var level = playerBusinessData.HasBusiness(businessType) ? playerBusinessData.GetLevel(businessType) :
                (configData.PreOpened? 1:0);
            
            AddPoolComponent<LevelComponent>(entity).Value = level;
            ref var levelUpPriceComponent = ref AddPoolComponent<LevelUpPriceComponent>(entity);
            levelUpPriceComponent.Value=FormulasUtils.CalculateNextLevelPrice(level, configData.BasePrice);
            levelUpPriceComponent.BaseValue = configData.BasePrice;

            return level;
        }

        private void CreateIncomeComponents(int entity, int level, BusinessData configData,
            BusinessModelMediator playerBusinessData, bool businessBought, BusinessType businessType)
        {
            ref var incomeMultiplierComponent = ref AddPoolComponent<IncomeMultiplierComponent>(entity);
            foreach (var upgradeType in playerBusinessData.GetUpgrades(businessType))
            {
                incomeMultiplierComponent.ValuePercent += configData.TryGetUpgrade(upgradeType).IncomeMultiplier;
            }
            
            ref var incomeComponent = ref AddPoolComponent<IncomeComponent>(entity);
            incomeComponent.Capacity = FormulasUtils.CalculateIncome(level, configData.BaseIncome,
                incomeMultiplierComponent.ValuePercent);
            incomeComponent.IncomeDuration = configData.IncomeDuration;
            incomeComponent.BaseIncome = configData.BaseIncome;
            incomeComponent.CurrentIncomeNormalized = businessBought? playerBusinessData.GetNormalizedIncomeProgress(businessType) :0;
        }
        
        private void CreateUpgradeEntities(BusinessType businessType, bool isActive)
        {
            var configData = _dataConfig.GetBusinessData(businessType);

            foreach (var upgradeData in configData.Upgrades)
            {
                var entity = _world.NewEntity();
                
                var title = _termsConfig.GetUpgradeName(upgradeData.UpgradeType);
                
                AddPoolComponent<TitleComponent>(entity).Value = title;
                
                AddPoolComponent<UpgradePriceComponent>(entity).Value = upgradeData.Price;
                AddPoolComponent<BusinessTypeComponent>(entity).Value = businessType;
                AddPoolComponent<UpgradeTypeComponent>(entity).Value = upgradeData.UpgradeType;
                AddPoolComponent<IncomeMultiplierComponent>(entity).ValuePercent = upgradeData.IncomeMultiplier;

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