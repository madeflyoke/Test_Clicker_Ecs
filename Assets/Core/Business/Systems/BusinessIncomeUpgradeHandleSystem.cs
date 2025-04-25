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
    
        private EcsFilterInject<Inc<BusinessUpgradeRequestComponent, IncomeMultiplierComponent, UpgradeTypeComponent,
            BusinessTypeComponent, ActiveStateComponent, UpgradePriceComponent>> _requestCallersFilter;
        
        private EcsPoolInject<MoneyCurrencyChangedRequestComponent> _moneyRequestPool;
        
        private EcsPoolInject<NotifyValueChangedComponent<UpgradeTypeComponent>> _notifiers;
        private EcsFilterInject<Inc<BusinessTypeComponent, IncomeMultiplierComponent>, Exc<UpgradeTypeComponent>> _notifiersFilter;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _requestCallersFilter.Value)
            {
                ref var incomeMultiplier = ref _requestCallersFilter.Pools.Inc2.Get(entity).ValuePercent;
                ref var upgradeType = ref _requestCallersFilter.Pools.Inc3.Get(entity);
                
                ref var price = ref _requestCallersFilter.Pools.Inc6.Get(entity).Value;
                
                if (price>_playerDataService.Value.MoneyCurrencyMediator.GetValue())
                {
                    continue;
                }
                
                ref var businessType = ref _requestCallersFilter.Pools.Inc4.Get(entity).Value;
                _playerDataService.Value.BusinessMediator.AddUpgrade(businessType, upgradeType.Value);
                
                ref var moneyRequest = ref _moneyRequestPool.Value.Add(systems.GetWorld().NewEntity());
                moneyRequest.Value = price;
                moneyRequest.Operation = OperationType.Subtract;
                
                ApplyData(businessType, incomeMultiplier, upgradeType.Value);
                
                systems.GetWorld().GetPool<ActiveStateComponent>().Del(entity);
            }
        }
        
        private void ApplyData(BusinessType targetType, int additionalIncomeMultiplier, UpgradeType upgrade)
        {
            foreach (var notifierEntity in _notifiersFilter.Value)
            {
                ref var businessType = ref _notifiersFilter.Pools.Inc1.Get(notifierEntity);

                if (businessType.Value==targetType)
                {
                    ref var incomeMultiplier = ref _notifiersFilter.Pools.Inc2.Get(notifierEntity);
                    incomeMultiplier.ValuePercent+= additionalIncomeMultiplier;
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
