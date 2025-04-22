using Leopotam.EcsLite;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.EntityView {
   public static class GetPackedEntityExt {
      public static EcsPackedEntity          PackedEntity(this          global::Packages.LeoEcs.LeoEcsDebug.Runtime.View.EntityView entityView) => entityView.World.PackEntity(entityView.Entity);
      public static EcsPackedEntityWithWorld PackedEntityWithWorld(this global::Packages.LeoEcs.LeoEcsDebug.Runtime.View.EntityView entityView) => entityView.World.PackEntityWithWorld(entityView.Entity);
   }
}