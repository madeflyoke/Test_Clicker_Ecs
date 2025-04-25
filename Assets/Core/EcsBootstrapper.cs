using System.Collections.Generic;
using System.Linq;
using Core.Business.Components;
using Core.Business.Components.Events;
using Core.Business.Systems;
using Core.Business.Systems.Notify;
using Core.Business.Upgrades.Components;
using Core.Business.Upgrades.Systems;
using Core.Common.Components.Events;
using Core.Currency.Components;
using Core.Currency.Components.Events;
using Core.Currency.Systems;
using Core.Currency.Systems.Notify;
using Core.Interfaces;
using Core.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace Core
{
    public class EcsBootstrapper : MonoBehaviour
    {
        private EcsWorld _defaultWorld;
        private EcsSystems _systems;

        private List<IEcsInitialized> _registers;
        
        private bool _initialized;

        public void Initialize(ServicesProvider servicesProvider, params IEcsInitialized[] optionalRegisters)
        {
            _defaultWorld = new EcsWorld();
            _systems = new EcsSystems(_defaultWorld);

            InitializeRegisters(optionalRegisters.ToList());

            AddSystems();
            
            _systems.Inject(
                servicesProvider.PlayerDataService, 
                servicesProvider.GameDataProviderService);
            _systems.Init();
            _initialized = true;
        }

        private void InitializeRegisters(List<IEcsInitialized> registers)
        {
            foreach (var r in registers)
            {
                r.EcsInitialize(_defaultWorld);
            }
        }

        private void AddSystems()
        {
            _systems
                .Add(new BusinessLevelUpSystem())
                .DelHere<BusinessLevelUpRequestComponent>()
                .Add(new BusinessBuySystem())
                .DelHere<BusinessBuyRequestComponent>()

                .Add(new BusinessIncomeUpgradeHandleSystem())
                .DelHere<BusinessUpgradeRequestComponent>()
                .Add(new BusinessIncomeCapacityChangeSystem())
                .Add(new BusinessLevelPriceChangeSystem())
                
                .Add(new MoneyCurrencyIncomeProgressSystem())
                .Add(new MoneyCurrencyIncomeProgressBarFillSystem())
                
                .Add(new MoneyCurrencyChangeSystem())
                .DelHere<MoneyCurrencyChangedRequestComponent>()

                .Add(new BusinessLevelNotifyListenersSystem())
                .DelHere<NotifyValueChangedComponent<LevelComponent>>()
                .Add(new BusinessUpgradeNotifySystem())
                .DelHere<NotifyValueChangedComponent<UpgradeTypeComponent>>()
                .Add(new MoneyCurrencyNotifyListenersSystem())
                .DelHere<NotifyValueChangedComponent<MoneyInfoComponent>>()
                .Add(new MoneyCurrencyProgressBarNotifyListenersSystem())
                .DelHere<NotifyValueChangedComponent<MoneyCurrencyProgressBarComponent>>()
                .Add(new BusinessIncomeCapacityNotifyListenersSystem())
                .DelHere<NotifyValueChangedComponent<IncomeComponent>>()
                .Add(new BusinessLevelPriceNotifyListenersSystem())
                .DelHere<NotifyValueChangedComponent<LevelUpPriceComponent>>()
                
                .DelHere<NotifyFullRefreshComponent>();

#if CUSTOM_DEBUG
            _systems
                .Add(new EcsSystemsDebugSystem())
                .Add(new EcsWorldDebugSystem());
#endif
        }
        
        private void Update()
        {
            if (_initialized)
               _systems.Run();
        }
        
        private void OnDestroy()
        {
            _defaultWorld?.Destroy();
            _defaultWorld = null;
            
            _systems?.Destroy();
            _systems = null;
        }
    }
}
