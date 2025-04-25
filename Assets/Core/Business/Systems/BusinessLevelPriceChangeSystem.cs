using Core.Business.Components;
using Core.Common.Components.Events;
using Core.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Business.Systems
{
    public class BusinessLevelPriceChangeSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<NotifyValueChangedComponent<LevelComponent>, LevelUpPriceComponent>> _levelPriceFilter; 
        
        private EcsPoolInject<NotifyValueChangedComponent<LevelUpPriceComponent>> _notifiers;
    
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _levelPriceFilter.Value)
            {
                ref var levelComponent = ref _levelPriceFilter.Pools.Inc1.Get(entity).ValueSource;
                ref var priceComponent = ref _levelPriceFilter.Pools.Inc2.Get(entity);

                priceComponent.Value = FormulasUtils.CalculateNextLevelPrice(levelComponent.Value, priceComponent.BaseValue);
                Notify(entity, priceComponent.Value);
            }
        }

        private void Notify(int target, double value)
        {
            ref var notifier = ref _notifiers.Value.Add(target);
            notifier.ValueSource.Value = value;
        }
    }
}
