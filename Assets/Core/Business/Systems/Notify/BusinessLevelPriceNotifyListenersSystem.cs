using Core.Business.Components;
using Core.Common.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Core.Business.Systems.Notify
{
    public class BusinessLevelPriceNotifyListenersSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<ValueChangedListenerComponent<LevelUpPriceComponent, double>, LevelUpPriceComponent>>
            _listenersFilter;
        
        private EcsPoolInject<NotifyFullRefreshComponent> _fullRefreshPool;
        private EcsPoolInject<NotifyValueChangedComponent<LevelUpPriceComponent>> _notifiersPool;
        
        
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
            foreach (var entity in _listenersFilter.Value)
            {
                ref var listener = ref _listenersFilter.Pools.Inc1.Get(entity).Listener;
                
                if (_fullRefreshPool.Value.Has(entity))
                {
                    listener.OnValueChanged(_listenersFilter.Pools.Inc2.Get(entity).Value);
                }
                else if(_notifiersPool.Value.Has(entity))
                {
                    listener.OnValueChanged(_notifiersPool.Value.Get(entity).ValueSource.Value);
                }
            }
        }
    }
}
