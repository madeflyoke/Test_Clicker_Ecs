using Leopotam.EcsLite;

namespace Packages.LeoEcs.VoodyConversion.MonoHelpers
{
    public interface IConvertToEntity
    {
        void Convert(int entity, EcsWorld world);
    }
}
