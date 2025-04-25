using Core.Business.Components;
using Core.Currency.Components;
using Core.Currency.Components.Events;
using Core.Utils.Enums;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Core.Currency.Systems
{
    public class MoneyCurrencyIncomeProgressSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<IncomeComponent, LevelComponent>> _targetFilter;
        
        private EcsPoolInject<IncomeComponent> _incomePool;
        private EcsPoolInject<LevelComponent> _levelPool;
        
        private EcsPoolInject<MoneyCurrencyChangedRequestComponent> _requestPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _targetFilter.Value)
            {
                ref var level = ref _levelPool.Value.Get(entity);

                if (level.Value<1) //business not bought check for now
                {
                    continue;
                }
                
                ref var income = ref _incomePool.Value.Get(entity);
                income.CurrentIncomeNormalized += Time.deltaTime/ income.IncomeDuration;
                if (income.CurrentIncomeNormalized>=1f)
                {
                    income.CurrentIncomeNormalized = 1f; //clamp
                    
                    ref var requestComponent = ref _requestPool.Value.Add(systems.GetWorld().NewEntity());
                    requestComponent.Operation = OperationType.Add;
                    requestComponent.Value = income.Capacity;
                    
                    income.CurrentIncomeNormalized = 0;
                }
            }
        }
    }
}
