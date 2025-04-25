using Core.Common.Components.Events;
using Core.Currency.Components;
using Core.Services.PlayerData;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Core.Business.Systems.Notify
{
    public class MoneyCurrencyNotifyListenersSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilterInject<Inc<ValueChangedListenerComponent<MoneyInfoComponent, double>>>
            _listenersFilter;
        
        private EcsFilterInject<Inc<NotifyValueChangedComponent<MoneyInfoComponent>>> _notifiersPoolFilter;
        private EcsFilterInject<Inc<NotifyFullRefreshComponent>> _fullRefreshFilter;
        
        private EcsCustomInject<PlayerDataService> _playerDataService;
        
        public void Init(IEcsSystems systems)
        {
            NotifyListeners();
        }

        public void Run(IEcsSystems systems)
        {
            NotifyListeners();
        }
        
        private void NotifyListeners()
        {
            foreach (var _ in _fullRefreshFilter.Value)
            {
                foreach (var listenerEntity in _listenersFilter.Value)
                {
                    ref var listener = ref _listenersFilter.Pools.Inc1.Get(listenerEntity).Listener;
                    listener.OnValueChanged(_playerDataService.Value.MoneyCurrencyMediator.GetValue());
                }
            }

            foreach (var entity in _notifiersPoolFilter.Value)
            {
                if (_fullRefreshFilter.Pools.Inc1.Has(entity))
                {
                    continue;
                }
                foreach (var listenerEntity in _listenersFilter.Value)
                {
                    ref var listener = ref _listenersFilter.Pools.Inc1.Get(listenerEntity).Listener;
                    listener.OnValueChanged(_notifiersPoolFilter.Pools.Inc1.Get(entity).ValueSource.Value);
                }
            }
        }
    }
}