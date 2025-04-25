using Core.Business.Components;
using Core.Business.Data;
using Core.Business.Enums;
using Core.Common.Components;
using Core.Currency.Components;
using Core.Factory.Interfaces;
using Core.Services;
using Core.Services.PlayerData;
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
            
            var configData = _dataConfig.GetBusinessData(businessType); //todo blueprints
            var title = _termsConfig.GetBusinessName(businessType);
            
            var entity = _world.NewEntity();

            var level = playerBusinessData.HasBusiness(businessType) ? playerBusinessData.GetLevel(businessType) :
                (configData.PreOpened? 1:0);
            
            AddPoolComponent<LevelComponent>(entity).Value = level;
            ref var levelUpPriceComponent = ref AddPoolComponent<LevelUpPriceComponent>(entity);
            levelUpPriceComponent.Value=FormulasUtils.CalculateNextLevelPrice(level, configData.BasePrice);
            levelUpPriceComponent.BaseValue = configData.BasePrice;
            
            AddPoolComponent<BusinessTitleComponent>(entity).Value = title;
            AddPoolComponent<BusinessTypeComponent>(entity).Value = businessType;
            
            ref var incomeComponent = ref AddPoolComponent<IncomeComponent>(entity);
            incomeComponent.Capacity = FormulasUtils.CalculateIncome(level, configData.BaseIncome);//TODO insert upgrades
            incomeComponent.IncomeDuration = configData.IncomeDuration;
            incomeComponent.BaseIncome = configData.BaseIncome;

            ref var progressBarComponent = ref AddPoolComponent<MoneyCurrencyProgressBarComponent>(entity);
            progressBarComponent.MaxValue = 1f;
            
            return entity;
        }

        private ref T AddPoolComponent<T>(int entity) where T : struct
        {
            var pool = _world.GetPool<T>();
            return ref pool.Add(entity);
        }
    }
}