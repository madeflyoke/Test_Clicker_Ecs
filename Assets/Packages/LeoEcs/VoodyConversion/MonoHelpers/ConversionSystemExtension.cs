using Leopotam.EcsLite;
using Packages.LeoEcs.VoodyConversion.World.Systems;

namespace Packages.LeoEcs.VoodyConversion.MonoHelpers
{
    public static class ConversionSystemExtension
    {
        public static EcsSystems ConvertScene(this EcsSystems ecsSystems)
        {
            ecsSystems.Add(new WorldInitSystem());
            return ecsSystems;
        }
    }
}
