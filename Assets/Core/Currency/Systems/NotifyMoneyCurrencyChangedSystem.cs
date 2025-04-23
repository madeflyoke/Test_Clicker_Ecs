using Core.Currency.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Currency.Systems
{
    public class NotifyMoneyCurrencyChangedSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorldInject _defaultWorld;
        private EcsFilterInject<Inc<MoneyChangedListenerComponent>> _listenersFilter;
        private EcsPoolInject<MoneyChangedListenerComponent> _listenersPool;

        private EcsPoolInject<NotifyMoneyCurrencyChangedComponent> _notifiersPool;
        private EcsFilterInject<Inc<NotifyMoneyCurrencyChangedComponent>> _notifiersFilter;
        
        public void Init(IEcsSystems systems)
        {
            Notify();
        }
        
        public void Run(IEcsSystems systems)
        {
            Notify();
        }

        private void Notify()
        {
            foreach (var notifierEntity in _notifiersFilter.Value)
            {
                ref var notifier = ref _notifiersPool.Value.Get(notifierEntity);

                foreach (var listenerEntity in _listenersFilter.Value)
                {
                    ref var listener = ref _listenersPool.Value.Get(listenerEntity);
                    listener.Value.OnMoneyChanged(notifier.Value);
                }
            }
        }
    }
}
