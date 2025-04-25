using Core.Business.Components;
using Core.Currency.Components;
using Core.Currency.Components.Events;
using Core.Services.PlayerData;
using Core.Services.PlayerData.Business;
using Core.Utils.Enums;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Core.Business.Systems
{
    public class BusinessIncomeProgressSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsCustomInject<PlayerDataService> _playerDataService;
        private BusinessModelMediator _businessModelMediator;
        
        private EcsFilterInject<Inc<IncomeComponent, ActiveStateComponent, BusinessTypeComponent>> _targetFilter;
        
        private EcsPoolInject<MoneyCurrencyChangedRequestComponent> _requestPool;
        
        public void Init(IEcsSystems systems)
        {
            _businessModelMediator = _playerDataService.Value.BusinessMediator;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _targetFilter.Value)
            {
                ref var income = ref _targetFilter.Pools.Inc1.Get(entity);
                income.CurrentIncomeNormalized += Time.deltaTime/ income.IncomeDuration;
                
                ref var businessType = ref _targetFilter.Pools.Inc3.Get(entity).Value;
                _businessModelMediator.SetNormalizedIncomeProgress(businessType, income.CurrentIncomeNormalized);
                
                if (income.CurrentIncomeNormalized>=1f)
                {
                    ref var requestComponent = ref _requestPool.Value.Add(systems.GetWorld().NewEntity());
                    requestComponent.Operation = OperationType.Add;
                    requestComponent.Value = income.Capacity;
                    
                    income.CurrentIncomeNormalized = 0;
                }
            }
        }
    }
}
