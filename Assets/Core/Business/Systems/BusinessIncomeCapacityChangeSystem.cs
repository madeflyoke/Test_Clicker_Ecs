using Core.Business.Components;
using Core.Business.Upgrades.Components;
using Core.Common.Components.Events;
using Core.Currency.Components;
using Core.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Business.Systems
{
    public class BusinessIncomeCapacityChangeSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<IncomeComponent, IncomeMultiplierComponent, LevelComponent, BusinessTypeComponent>> 
            _targetFilter;
        
        private EcsPoolInject<NotifyValueChangedComponent<LevelComponent>> _requestsByLevel; 
        private EcsPoolInject<NotifyValueChangedComponent<UpgradeTypeComponent>> _requestsByMultiplier; 
        
        private EcsPoolInject<NotifyValueChangedComponent<IncomeComponent>> _notifiers;
    
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _targetFilter.Value)
            {
                ref var levelComponent = ref _targetFilter.Pools.Inc3.Get(entity);
                ref var incomeComponent = ref _targetFilter.Pools.Inc1.Get(entity);
                ref var incomeMultiplierComponent = ref _targetFilter.Pools.Inc2.Get(entity);
                
                incomeComponent.Capacity = FormulasUtils.CalculateIncome(levelComponent.Value, 
                    incomeComponent.BaseIncome, incomeMultiplierComponent.ValuePercent);

                if (_requestsByLevel.Value.Has(entity) || _requestsByMultiplier.Value.Has(entity))
                {
                    Notify(entity, incomeComponent.Capacity);
                }
            }
        }

        private void Notify(int target, double value)
        {
            ref var notifier = ref _notifiers.Value.Add(target);
            notifier.ValueSource.Capacity = value;
        }
    }
}
