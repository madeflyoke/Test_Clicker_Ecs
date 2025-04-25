using Core.Common.Components.Events;
using Core.Currency.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Currency.Systems.Notify
{
    public class MoneyCurrencyProgressBarNotifyListenersSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilterInject<Inc<ValueChangedListenerComponent<MoneyCurrencyProgressBarComponent, float>, MoneyCurrencyProgressBarComponent>>
            _listenersFilter;
        
        private EcsPoolInject<NotifyFullRefreshComponent> _fullRefreshPool;
        private EcsPoolInject<NotifyValueChangedComponent<MoneyCurrencyProgressBarComponent>> _notifiersPool;
        
        
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
                    listener.OnValueChanged(_listenersFilter.Pools.Inc2.Get(entity).CurrentValue);
                }
                else if(_notifiersPool.Value.Has(entity))
                {
                    listener.OnValueChanged(_notifiersPool.Value.Get(entity).ValueSource.CurrentValue);
                }
            }
        }
    }
}