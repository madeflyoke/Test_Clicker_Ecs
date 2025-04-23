using Leopotam.EcsLite;

namespace Packages.LeoEcs.VoodyConversion.MonoHelpers
{
    public static class WorldHandler
    {
        private static EcsWorld world;

        public static void Init(EcsWorld ecsWorld)
        {
            world = ecsWorld;
        }

        public static EcsWorld GetMainWorld()
        {
            return world;
        }

        public static void Destroy()
        {
            world = null;
        }
    }
}
