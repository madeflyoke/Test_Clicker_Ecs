using Core.Currency.Components.Events;
using Core.Currency.Systems;
using Core.Services.Configs;
using Core.Services.PlayerData;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Packages.LeoEcs.LeoEcsDebug.Runtime;
using Packages.LeoEcs.VoodyConversion.MonoHelpers;
using UnityEngine;

namespace Core
{
    public class EcsBootstrapper : MonoBehaviour
    {
        private EcsWorld _defaultWorld;
        private EcsSystems _systems;
        
        private bool _initialized;

        public void Initialize(PlayerDataService playerDataService, GameDataProviderService gameDataProviderService)
        {
            _defaultWorld = new EcsWorld();
            _systems = new EcsSystems(_defaultWorld);
            
            AddSystems();
            _systems.ConvertScene();
            _systems.Inject(
                playerDataService, 
                gameDataProviderService);
            _systems.Init();
            _initialized = true;
        }

        private void AddSystems()
        {
            _systems
                .Add(new MoneyCurrencyChangeSystem())
                .DelHere<MoneyCurrencyChangedRequestComponent>()
                .Add(new NotifyMoneyCurrencyChangedSystem())
                .DelHere<NotifyMoneyCurrencyChangedComponent>()
                
                .Add(new EcsSystemsDebugSystem())
                .Add(new EcsWorldDebugSystem());
        }
        
        private void Update()
        {
            if (_initialized)
               _systems.Run();
        }
        
        private void OnDestroy()
        {
            _defaultWorld?.Destroy();
            _defaultWorld = null;
            
            _systems?.Destroy();
            _systems = null;
        }
    }
}
