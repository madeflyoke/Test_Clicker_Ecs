using Core.Business.Components;
using Core.Common.Components.Events;
using Core.Currency.Components;
using Core.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Business.Systems
{
    public class BusinessIncomeCapacityChangeSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<NotifyValueChangedComponent<LevelComponent>, IncomeComponent>> _incomesFilter; 
        
        private EcsPoolInject<NotifyValueChangedComponent<IncomeComponent>> _notifiers;
    
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _incomesFilter.Value)
            {
                ref var levelComponent = ref _incomesFilter.Pools.Inc1.Get(entity).ValueSource;
                ref var incomeComponent = ref _incomesFilter.Pools.Inc2.Get(entity);

                incomeComponent.Capacity = FormulasUtils.CalculateIncome(levelComponent.Value, incomeComponent.BaseIncome);
                Notify(entity, incomeComponent.Capacity);
            }
        }

        private void Notify(int target, double value)
        {
            ref var notifier = ref _notifiers.Value.Add(target);
            notifier.ValueSource.Capacity = value;
        }
    }
}
