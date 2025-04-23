using System;
using Leopotam.EcsLite;
using Packages.LeoEcs.VoodyConversion.MonoHelpers;
using Packages.LeoEcs.VoodyConversion.World.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.LeoEcs.VoodyConversion.World.Systems
{
    /// This class handle global init to ECS World

#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif

    class WorldInitSystem : IEcsPreInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsPool<InstantiateComponent> _instantiatePool;
        private EcsFilter _filter;
        private EcsWorld _baseWorld;

        public void PreInit(IEcsSystems systems)
        {
            var convertableGameObjects = Object.FindObjectsOfType<ConvertToEntity>();
            // Iterate throught all gameobjects, that has ECS Components
            foreach (var convertable in convertableGameObjects)
            {
                AddEntity(convertable.gameObject, systems, convertable.GetWorldName());
            }

            _baseWorld = systems.GetWorld();
            _filter = _baseWorld.Filter<InstantiateComponent>().End();
            _instantiatePool = _baseWorld.GetPool<InstantiateComponent>();

            // After adding all entitites from the begining of the scene, we need to handle global World value
            WorldHandler.Init(_baseWorld);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref InstantiateComponent instantiate = ref _instantiatePool.Get(i);
                if (instantiate.gameObject)
                {
                    AddEntity(instantiate.gameObject, systems, instantiate.worldName);
                }

                _baseWorld.DelEntity(i);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            WorldHandler.Destroy();
        }

        // Creating New Entity with components function
        private void AddEntity(GameObject gameObject, IEcsSystems systems, String worldName)
        {
            var nameValue = worldName == "" ? null : worldName;
            var spawnWorld = systems.GetWorld(nameValue);
            int entity = spawnWorld.NewEntity();
            ConvertToEntity convertComponent = gameObject.GetComponent<ConvertToEntity>();
            if (convertComponent)
            {
                foreach (var component in gameObject.GetComponents<Component>())
                {
                    if (component is IConvertToEntity entityComponent)
                    {
                        // Adding Component to entity
                        entityComponent.Convert(entity, spawnWorld);
                        Object.Destroy(component);
                    }
                }
		
	            convertComponent.setProccessed();
                switch (convertComponent.GetConvertMode())
                {
                    case ConvertMode.ConvertAndDestroy:
                        Object.Destroy(gameObject);
                        break;
                    case ConvertMode.ConvertAndInject:
                        Object.Destroy(convertComponent);
                        break;
                    case ConvertMode.ConvertAndSave:
                        convertComponent.Set(entity, spawnWorld);
                        break;
                }
            }
        }
    }
}