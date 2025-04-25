using Leopotam.EcsLite;

namespace Core.Interfaces
{
    public interface IEcsInitialized
    {
        public void EcsInitialize(EcsWorld relatedWorld);
    }
}
