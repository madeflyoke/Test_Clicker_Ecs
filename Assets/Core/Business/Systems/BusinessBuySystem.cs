using Core.Business.Components;
using Core.Business.Components.Events;
using Core.Business.Upgrades.Components;
using Core.Services;
using Core.Services.PlayerData;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Business.Systems
{
    public class BusinessBuySystem : IEcsRunSystem
    {
        private EcsCustomInject<PlayerDataService> _playerDataService;

        private EcsFilterInject<Inc<BusinessBuyRequestComponent,BusinessTypeComponent>> _requestFilter;

        private EcsFilterInject<Inc<BusinessTypeComponent>, Exc<ActiveStateComponent>> _toActivatePartsFilter;
        private EcsPoolInject<ActiveStateComponent> _activeStatePool;
        
        private EcsPoolInject<BusinessTypeComponent> _typesPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _requestFilter.Value)
            {
                ref var typeComponent = ref _typesPool.Value.Get(entity);
                _playerDataService.Value.BusinessMediator.AddNewBusiness(typeComponent.Value);

                foreach (var activateEntity in _toActivatePartsFilter.Value)
                {
                    ref var businessType = ref _toActivatePartsFilter.Pools.Inc1.Get(activateEntity).Value;

                    if (businessType==typeComponent.Value)
                    {
                        _activeStatePool.Value.Add(activateEntity); //upgrades and business self
                    }
                }
            }
        }
    }
}
