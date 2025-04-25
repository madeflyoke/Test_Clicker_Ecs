using System.Collections.Generic;
using Core.Business.Components;
using Core.Business.Components.Events;
using Core.Business.Data;
using Core.Business.Enums;
using Core.Common.Components;
using Core.Common.Components.Events;
using Core.Common.Interfaces;
using Core.Currency.Components;
using Core.Currency.Components.Events;
using Core.Factory.Interfaces;
using Core.Services;
using Core.Terms;
using Core.UI.Gameplay.Business;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Factory
{
    public class BusinessViewFactory : IEntityFactory
    {
        private EcsWorld _world;
        private BusinessElementView _businessElementViewPrefab;

        public void Initialize(EcsWorld world, ServicesProvider servicesProvider)
        {
            _world = world;

            _businessElementViewPrefab =
                servicesProvider.GameDataProviderService.ViewPrefabsProvider.BusinessElementViewPrefab;
        }

        public BusinessElementView ConstructNew(Transform parent)
        {
            var instance = Object.Instantiate(_businessElementViewPrefab, parent);
            return instance;
        }
        
        public void ApplyEntityToView(BusinessElementView view, int dataEntity)
        {
            var title = _world.GetPool<BusinessTitleComponent>().Get(dataEntity).Value;
            view.SetTitle(title);

            var incomeValue = _world.GetPool<IncomeComponent>().Get(dataEntity).MaxValue;
            view.IncomeInfoView.SetIncomeValue(incomeValue);
            
            AddPoolComponent<ValueChangedListenerComponent<MoneyCurrencyProgressBarComponent, float>>(dataEntity).Listener =
                view.MoneyCurrencyProgressBar;

            var levelPrice = _world.GetPool<LevelUpPriceComponent>().Get(dataEntity).Value;
            
            view.LevelUpButtonView.Setup(()=>AddPoolComponent<BusinessLevelUpRequestComponent>(dataEntity),
                levelPrice);
            
            AddPoolComponent<ValueChangedListenerComponent<LevelComponent, int>>(dataEntity).Listener =
                view.LevelInfoView;

            ApplyRefresh(dataEntity);
        }


        private void ApplyRefresh(int entity)
        {
            AddPoolComponent<NotifyFullRefreshComponent>(entity);
        }
        
        private ref T AddPoolComponent<T>(int entity) where T : struct
        {
            var pool = _world.GetPool<T>();
            return ref pool.Add(entity);
        }
    }
}