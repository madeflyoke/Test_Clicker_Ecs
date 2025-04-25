using System.Collections.Generic;
using Core.Business.Data;
using Core.Business.Enums;
using Core.Interfaces;
using Core.Services;
using Leopotam.EcsLite;

namespace Core.Factory
{
    public class BusinessEntitiesBuilder : IEcsInitialized
    {
        public Dictionary<BusinessType, int> Entities { get;private set; }
        
        private BusinessEntityFactory _factory;
        private BusinessConfig _config;
        
        public BusinessEntitiesBuilder(ServicesProvider servicesProvider)
        {
            Entities = new Dictionary<BusinessType, int>();   
            
            _factory = servicesProvider.EntityFactoriesService.GetEntityFactory<BusinessEntityFactory>();
            _config = servicesProvider.GameDataProviderService.BusinessConfig;
        }

        public void EcsInitialize(EcsWorld relatedWorld)
        {
            Build();
        }
        
        private void Build()
        {
            foreach (var kvp in _config.GetAllBusinessData())
            {
                Entities.Add(kvp.Key, _factory.Construct(kvp.Key));
            }
        }
    }
}
