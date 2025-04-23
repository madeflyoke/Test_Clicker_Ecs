using Core.Currency.Components.Events;
using Core.Services.PlayerData;
using Core.Services.PlayerData.Currency;
using Core.Utils.Enums;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Core.Currency.Systems
{
    public class MoneyCurrencyChangeSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorldInject _defaultWorld;
        private EcsCustomInject<PlayerDataService> _playerDataService;
        
        private EcsFilterInject<Inc<MoneyCurrencyChangedRequestComponent>> _moneyCurrencyFilter;
        private EcsPoolInject<MoneyCurrencyChangedRequestComponent> _moneyRequestsPool;

        private MoneyCurrencyModelMediator _moneyCurrencyModelMediator;
        
        public void Init(IEcsSystems systems)
        {
            _moneyCurrencyModelMediator = _playerDataService.Value.MoneyCurrencyMediator;
            systems.GetWorld().GetPool<NotifyMoneyCurrencyChangedComponent>().Add(systems.GetWorld().NewEntity()).Value = _moneyCurrencyModelMediator.GetValue();
        }
        
        public void Run(IEcsSystems systems)
        {
            var result = _moneyCurrencyModelMediator.GetValue();
            bool changed = false;
            
            foreach (var entity in _moneyCurrencyFilter.Value)
            {
                ref var requestComponent = ref _moneyRequestsPool.Value.Get(entity);
                var requestValue = requestComponent.Value;

                if (requestValue!=0)
                {
                    changed = true;
                }
                
                switch (requestComponent.Operation)
                {
                    case OperationType.Add:
                        result += requestValue;
                        break;
                    case OperationType.Subtract:
                        result -= requestValue;
                        break;
                    case OperationType.Set:
                        result = requestValue;
                        break;
                }
                
                _moneyCurrencyModelMediator.Operate(result, OperationType.Set);
            }
               
            if (changed)
               systems.GetWorld().GetPool<NotifyMoneyCurrencyChangedComponent>().Add(systems.GetWorld().NewEntity()).Value = result;
        }
    }
}
