using Core.Business.Components;
using Core.Business.Components.Events;
using Core.Common.Components.Events;
using Core.Currency.Components.Events;
using Core.Services.PlayerData;
using Core.Utils.Enums;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Core.Business.Systems
{
    public class BusinessLevelUpSystem : IEcsRunSystem
    {
        private EcsCustomInject<PlayerDataService> _playerDataService;
        
        private EcsFilterInject<Inc<BusinessLevelUpRequestComponent, LevelComponent, LevelUpPriceComponent, 
            BusinessTypeComponent>> _requestFilter;
        
        private EcsPoolInject<MoneyCurrencyChangedRequestComponent> _moneyRequestPool;
        
        private EcsPoolInject<NotifyValueChangedComponent<LevelComponent>> _notifiers;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _requestFilter.Value)
            {
                ref var levelUpPrice = ref _requestFilter.Pools.Inc3.Get(entity);
                
                if (levelUpPrice.Value>_playerDataService.Value.MoneyCurrencyMediator.GetValue())
                {
                    continue;
                }
                
                ref var moneyRequest = ref _moneyRequestPool.Value.Add(systems.GetWorld().NewEntity());
                moneyRequest.Value = levelUpPrice.Value;
                moneyRequest.Operation = OperationType.Subtract;
                
                ref var levelComponent = ref _requestFilter.Pools.Inc2.Get(entity);
                if (levelComponent.Value==0)
                {
                    systems.GetWorld().GetPool<BusinessBuyRequestComponent>().Add(entity);
                }
                else //buy business means automatically add level
                {
                    ref var businessType = ref _requestFilter.Pools.Inc4.Get(entity).Value;
                    _playerDataService.Value.BusinessMediator.AddLevel(businessType);
                }
                
                levelComponent.Value++;
                
                Notify(entity, levelComponent.Value);
            }
        }
        
        private void Notify(int target, int value)
        {
            ref var notifier = ref _notifiers.Value.Add(target);
            notifier.ValueSource.Value = value;
        }
    }
}
