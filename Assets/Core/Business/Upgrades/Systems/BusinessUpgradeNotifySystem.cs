using Core.Business.Components;
using Core.Business.Upgrades.Components;
using Core.Common.Components.Events;
using Core.Services.PlayerData;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Business.Upgrades.Systems
{
    public class BusinessUpgradeNotifySystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsCustomInject<PlayerDataService> _playerDataService;
        
        private EcsFilterInject<Inc<CommonListenerComponent<UpgradeTypeComponent>, UpgradeTypeComponent,
            BusinessTypeComponent>>
            _listenersFilter;
        
        private EcsFilterInject<Inc<NotifyFullRefreshComponent, UpgradeTypeComponent,
            BusinessTypeComponent>> _fullRefreshNotifierFilter;
        
        private EcsFilterInject<Inc<NotifyValueChangedComponent<UpgradeTypeComponent>, BusinessTypeComponent>, 
                Exc<NotifyFullRefreshComponent>>
            _notifiersFilter;
        
        public void Init(IEcsSystems systems)
        {
            NotifyListeners();
        }

        public void Run(IEcsSystems systems)
        {
            NotifyListeners();
        }
        
        private void NotifyListeners() //comparison of upgrade type/business type tags
        {
            foreach (var notifier in _fullRefreshNotifierFilter.Value)
            {
                ref var notifierUpgrade =
                    ref _fullRefreshNotifierFilter.Pools.Inc2.Get(notifier).Value;
                ref var notifierBusinessType = ref _fullRefreshNotifierFilter.Pools.Inc3.Get(notifier).Value;
                
                if (_playerDataService.Value.BusinessMediator.GetUpgrades(notifierBusinessType).Contains(notifierUpgrade))
                {
                    foreach (var listenerEntity in _listenersFilter.Value)
                    {
                        ref var listenerUpgrade = ref _listenersFilter.Pools.Inc2.Get(listenerEntity).Value;
                        ref var listenerBusinessType = ref _listenersFilter.Pools.Inc3.Get(listenerEntity).Value;
            
                        if (notifierUpgrade==listenerUpgrade && notifierBusinessType==listenerBusinessType)
                        {
                            ref var listener = ref _listenersFilter.Pools.Inc1.Get(listenerEntity);
                            listener.NotifyListener.OnNotifyTriggered();
                        }
                    }
                }
            }
            
            foreach (var notifier in _notifiersFilter.Value)
            {
                ref var notifierUpgrade =
                    ref _notifiersFilter.Pools.Inc1.Get(notifier).ValueSource.Value;
                ref var notifierBusinessType = ref _notifiersFilter.Pools.Inc2.Get(notifier).Value;
                
                foreach (var listenerEntity in _listenersFilter.Value)
                {
                    ref var listenerUpgrade = ref _listenersFilter.Pools.Inc2.Get(listenerEntity).Value;
                    ref var listenerBusinessType = ref _listenersFilter.Pools.Inc3.Get(listenerEntity).Value;

                    if (notifierUpgrade==listenerUpgrade && notifierBusinessType==listenerBusinessType)
                    {
                        ref var listener = ref _listenersFilter.Pools.Inc1.Get(listenerEntity);
                        listener.NotifyListener.OnNotifyTriggered();
                    }
                }
            }
        }
    }
}
