using Core.Common.Components.Events;
using Core.Currency.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Business.Systems.Notify
{
    public class BusinessIncomeCapacityNotifyListenersSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilterInject<Inc<ValueChangedListenerComponent<IncomeComponent, double>, IncomeComponent>>
            _listenersFilter;
        
        private EcsPoolInject<NotifyFullRefreshComponent> _fullRefreshPool;
        private EcsPoolInject<NotifyValueChangedComponent<IncomeComponent>> _notifiersPool;
        
        
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
                    listener.OnValueChanged(_listenersFilter.Pools.Inc2.Get(entity).Capacity);
                }
                else if(_notifiersPool.Value.Has(entity))
                {
                    listener.OnValueChanged(_notifiersPool.Value.Get(entity).ValueSource.Capacity);
                }
            }
        }
    }
}