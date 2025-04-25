using Core.Business.Components;
using Core.Business.Components.Events;
using Core.Business.Enums;
using Core.Business.Upgrades.Components;
using Core.Common.Components.Events;
using Core.Currency.Components;
using Core.Currency.Components.Events;
using Core.Services.PlayerData;
using Core.Utils.Enums;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Core.Business.Systems
{
    public class BusinessIncomeUpgradeHandleSystem : IEcsRunSystem
    {
        private EcsCustomInject<PlayerDataService> _playerDataService;
    
        private EcsFilterInject<Inc<BusinessUpgradeRequestComponent, UpgradeComponent, UpgradeTypeComponent,
            BusinessTypeComponent, ActiveStateComponent>> _requestCallersFilter;
        
        private EcsPoolInject<MoneyCurrencyChangedRequestComponent> _moneyRequestPool;
        
        private EcsPoolInject<NotifyValueChangedComponent<UpgradeTypeComponent>> _notifiers;
        private EcsFilterInject<Inc<BusinessTypeComponent, IncomeMultiplierComponent>> _notifiersFilter;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _requestCallersFilter.Value)
            {
                ref var upgrade = ref _requestCallersFilter.Pools.Inc2.Get(entity);
                ref var upgradeType = ref _requestCallersFilter.Pools.Inc3.Get(entity);
                
                var price = upgrade.Price;
                
                if (price>_playerDataService.Value.MoneyCurrencyMediator.GetValue())
                {
                    continue;
                }
                
                ref var businessType = ref _requestCallersFilter.Pools.Inc4.Get(entity).Value;
                _playerDataService.Value.BusinessMediator.AddUpgrade(businessType, upgradeType.Value);
                
                ref var moneyRequest = ref _moneyRequestPool.Value.Add(systems.GetWorld().NewEntity());
                moneyRequest.Value = price;
                moneyRequest.Operation = OperationType.Subtract;
                
                ApplyData(businessType, upgrade, upgradeType.Value);
                
                systems.GetWorld().GetPool<ActiveStateComponent>().Del(entity);
            }
        }
        
        private void ApplyData(BusinessType targetType, UpgradeComponent data, UpgradeType upgrade)
        {
            foreach (var notifierEntity in _notifiersFilter.Value)
            {
                ref var businessType = ref _notifiersFilter.Pools.Inc1.Get(notifierEntity);

                if (businessType.Value==targetType)
                {
                    ref var incomeMultiplier = ref _notifiersFilter.Pools.Inc2.Get(notifierEntity);
                    incomeMultiplier.MultiplierPercent+= data.MultiplierPercent;
                    NotifyUpgrade(notifierEntity, upgrade);
                }
            }
        }

        private void NotifyUpgrade(int entity, UpgradeType value)
        {
            ref var notifier = ref _notifiers.Value.Add(entity);
            notifier.ValueSource.Value = value;
        }
    }
}
