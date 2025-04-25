using Core.Factory;
using Core.Services;
using Core.UI.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private ServicesProvider _servicesProvider;
        [SerializeField] private EcsBootstrapper _ecsBootstrapper;
        [SerializeField] private GameplayEntitiesViewBuilder _gameplayEntitiesViewBuilder;
        private BusinessEntitiesBuilder _entitiesBuilder;
        
        [Button]
        public void Initialize()
        {
            _servicesProvider.Initialize();
            
            _entitiesBuilder = new BusinessEntitiesBuilder(_servicesProvider);
            _gameplayEntitiesViewBuilder.Initialize(_servicesProvider);
            
            _ecsBootstrapper.Initialize(_servicesProvider, 
                _servicesProvider.EntityFactoriesService,
                _entitiesBuilder,
                _gameplayEntitiesViewBuilder);
            
            _gameplayEntitiesViewBuilder.BuildBalanceView();
            _gameplayEntitiesViewBuilder.BuildBusinessElementsView(_entitiesBuilder.Entities);
        }

        private void OnApplicationQuit()
        {
            _servicesProvider.PlayerDataService.Save();
        }
    }
}
