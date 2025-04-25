using System.Collections.Generic;
using Core.Business.Components;
using Core.Business.Components.Events;
using Core.Business.Upgrades.Components;
using Core.Common.Components;
using Core.Common.Components.Events;
using Core.Currency.Components;
using Core.Factory.Interfaces;
using Core.Services;
using Core.UI.Gameplay.Business;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Factory
{
    public class BusinessViewFactory : IEntityFactory
    {
        private EcsWorld _world;
        private BusinessElementView _businessElementViewPrefab;
        private UpgradeView _upgradeViewPrefab;

        public void Initialize(EcsWorld world, ServicesProvider servicesProvider)
        {
            _world = world;

            _businessElementViewPrefab =
                servicesProvider.GameDataProviderService.ViewPrefabsProvider.BusinessElementViewPrefab;
        }

        public BusinessElementView Construct(Transform parent)
        {
            var instance = Object.Instantiate(_businessElementViewPrefab, parent);
            return instance;
        }
        
        public void ApplyEntityToView(BusinessElementView view, int dataEntity)
        {
            var title = _world.GetPool<TitleComponent>().Get(dataEntity).Value;
            view.SetTitle(title);
            
            var levelPrice = _world.GetPool<LevelUpPriceComponent>().Get(dataEntity).Value;
            
            view.LevelUpButtonView.SetupClickAction(()=>AddPoolComponent<BusinessLevelUpRequestComponent>(dataEntity),
                levelPrice); //button handle component?
            AddPoolComponent<ValueChangedListenerComponent<LevelUpPriceComponent, double>>(dataEntity).Listener =
                view.LevelUpButtonView;
            AddPoolComponent<ValueChangedListenerComponent<MoneyCurrencyProgressBarComponent, float>>(dataEntity).Listener =
                view.MoneyCurrencyProgressBar;
            AddPoolComponent<ValueChangedListenerComponent<LevelComponent, int>>(dataEntity).Listener =
                view.LevelInfoView;
            AddPoolComponent<ValueChangedListenerComponent<IncomeComponent, double>>(dataEntity).Listener =
                view.IncomeInfoView;

            ApplyUpgradesView(dataEntity, view.UpgradeViews);
            
            ApplyRefresh(dataEntity);
        }

        private void ApplyUpgradesView(int businessEntity, List<UpgradeView> targets)
        {
            ref var sourceBusinessType = ref _world.GetPool<BusinessTypeComponent>().Get(businessEntity);

            var index = 0;
            
            foreach (var entity in _world.Filter<UpgradeTypeComponent>().Inc<BusinessTypeComponent>().End())
            {
                ref var upgradeBusinessType = ref _world.GetPool<BusinessTypeComponent>().Get(entity);
                if (upgradeBusinessType.Value==sourceBusinessType.Value)
                {
                    if (index >= targets.Count)
                    {
                        Debug.LogWarning("No upgrade view to apply data");
                        return;
                    }
                    
                    var target = targets[index];
                    var title = _world.GetPool<TitleComponent>().Get(entity).Value;
                    target.SetTitle(title);

                    var incomeMultiplier = _world.GetPool<IncomeMultiplierComponent>().Get(entity);

                    var upgradePrice = _world.GetPool<UpgradePriceComponent>().Get(entity).Value;
                    target.SetPriceText(upgradePrice);
                    target.SetIncomeValueText(incomeMultiplier.ValuePercent);
                    
                    target.SetupClickAction(()=>AddPoolComponent<BusinessUpgradeRequestComponent>(entity));

                    AddPoolComponent<CommonListenerComponent<UpgradeTypeComponent>>(entity).NotifyListener = target;
                    ApplyRefresh(entity);
                    index++;
                }
            }
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