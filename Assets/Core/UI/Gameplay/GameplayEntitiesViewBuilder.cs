using System.Collections.Generic;
using Core.Business.Components.Events;
using Core.Business.Enums;
using Core.Common.Components.Events;
using Core.Currency.Components;
using Core.Currency.Components.Events;
using Core.Factory;
using Core.Interfaces;
using Core.Services;
using Core.UI.Gameplay.Business;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.UI.Gameplay
{
    public class GameplayEntitiesViewBuilder : MonoBehaviour, IEcsInitialized
    {
        [SerializeField] private BalanceView _balanceView;
        [SerializeField] private BusinessElementsViewController _businessElementsViewController;
        
        private ServicesProvider _servicesProvider;
        private EcsWorld _world;

        public void Initialize(ServicesProvider servicesProvider)
        {
            _servicesProvider = servicesProvider;
        }
        
        public void EcsInitialize(EcsWorld relatedWorld)
        {
            _world = relatedWorld;
        }
        
        public void BuildBalanceView()
        {
            var factory = _servicesProvider.EntityFactoriesService.GetEntityFactory<CustomEntityFactory>();
            factory.GetNewEntity(out var entity);
            factory.AddPoolComponent<MoneyInfoComponent>(entity);
            ref var listener = ref factory.AddPoolComponent<ValueChangedListenerComponent<MoneyInfoComponent, double>>(entity);
            listener.Listener = _balanceView;
            factory.ApplyRefresh(entity);
        }

        public void BuildBusinessElementsView(Dictionary<BusinessType, int> businessEntities)
        {
            var viewFactory = _servicesProvider.EntityFactoriesService.GetEntityFactory<BusinessViewFactory>();
            _businessElementsViewController.Initialize(viewFactory, businessEntities);
        }
    }
}
