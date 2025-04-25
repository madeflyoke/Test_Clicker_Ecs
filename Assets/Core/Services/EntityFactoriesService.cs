using System.Collections.Generic;
using System.Linq;
using Core.Factory;
using Core.Factory.Interfaces;
using Core.Interfaces;
using Leopotam.EcsLite;

namespace Core.Services
{
    public class EntityFactoriesService : IEcsInitialized
    {
        private readonly List<IEntityFactory> _entityFactories;
        private readonly ServicesProvider _servicesProvider;
        
        public EntityFactoriesService(ServicesProvider servicesProvider)
        {
            _servicesProvider = servicesProvider;
            
            _entityFactories = new List<IEntityFactory>();
            _entityFactories.Add(new BusinessEntityFactory());
            _entityFactories.Add(new BusinessViewFactory());
            _entityFactories.Add(new CustomEntityFactory());
        }
        
        public void EcsInitialize(EcsWorld relatedWorld)
        {
            _entityFactories.ForEach(x=>x.Initialize(relatedWorld,_servicesProvider));
        }
        
        public T GetEntityFactory<T>() where T : IEntityFactory
        {
            return (T)_entityFactories.FirstOrDefault(x => x.GetType()==typeof(T));
        }
    }
}
