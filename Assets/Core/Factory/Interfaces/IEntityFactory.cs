using Core.Services;
using Leopotam.EcsLite;

namespace Core.Factory.Interfaces
{
    public interface IEntityFactory
    {
        public void Initialize(EcsWorld world, ServicesProvider servicesProvider);
    }
}
