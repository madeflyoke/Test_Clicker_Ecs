using Core.Business.Components;
using Core.Business.Components.Events;
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

        private EcsPoolInject<BusinessTypeComponent> _typesPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _requestFilter.Value)
            {
                ref var typeComponent = ref _typesPool.Value.Get(entity);
                _playerDataService.Value.BusinessMediator.AddNewBusiness(typeComponent.Value);
            }
        }
    }
}
