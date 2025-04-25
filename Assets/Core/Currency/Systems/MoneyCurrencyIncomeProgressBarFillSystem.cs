using Core.Business.Components.Events;
using Core.Common.Components;
using Core.Common.Components.Events;
using Core.Currency.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Currency.Systems
{
    public class MoneyCurrencyIncomeProgressBarFillSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<MoneyCurrencyProgressBarComponent, IncomeComponent>> _targetFilter;

        private EcsPoolInject<MoneyCurrencyProgressBarComponent> _progressBarPool;
        private EcsPoolInject<IncomeComponent> _incomePool;
        private EcsPoolInject<NotifyValueChangedComponent<MoneyCurrencyProgressBarComponent>> _notifiers;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _targetFilter.Value)
            {
                ref var bar = ref _progressBarPool.Value.Get(entity);
                ref var income = ref _incomePool.Value.Get(entity);

                bar.CurrentValue = income.CurrentIncomeNormalized;
                if (bar.CurrentValue >= bar.MaxValue)
                {
                    bar.CurrentValue = bar.MaxValue; //clamp (need?)
                }

                Notify(entity, bar.CurrentValue);
            }
        }

        private void Notify(int target, float value)
        {
            ref var notifier = ref _notifiers.Value.Add(target);
            notifier.ValueSource.CurrentValue = value;
        }
    }
}