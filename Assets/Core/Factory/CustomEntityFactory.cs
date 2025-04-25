using Core.Common.Components.Events;
using Core.Factory.Interfaces;
using Core.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Factory
{
    public class CustomEntityFactory :IEntityFactory
    {
        private EcsWorld _world;
        
        public void Initialize(EcsWorld world, ServicesProvider servicesProvider)
        {
            _world = world;
        }

        public CustomEntityFactory GetNewEntity(out int entity)
        {
            entity = _world.NewEntity();
            return this;
        }
        
        public ref T AddPoolComponent<T>(int entity) where T : struct
        {
            return ref _world.GetPool<T>().Add(entity);
        }

        public void ApplyRefresh(int entity)
        {
            AddPoolComponent<NotifyFullRefreshComponent>(entity);
        }
    }
}
