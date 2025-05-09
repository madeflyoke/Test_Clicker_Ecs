#if UNITY_EDITOR
using System;
using Leopotam.EcsLite;
using Packages.LeoEcs.LeoEcsDebug.Runtime.Extensions.Ecs.World;
using Packages.LeoEcs.LeoEcsDebug.Runtime.Extensions.String;
using Packages.LeoEcs.LeoEcsDebug.Runtime.Name;
using Packages.LeoEcs.LeoEcsDebug.Runtime.View;

namespace Packages.LeoEcs.LeoEcsDebug.Runtime {
   public sealed class EcsWorldDebugSystem : IEcsPreInitSystem, IEcsRunSystem, IEcsWorldEventListener {
      public string       DebugName    { get; }
      public string       WorldName    { get; }
      public EcsWorld     World        { get; private set; }
      public WorldView    View         { get; private set; }
      public NameSettings NameSettings { get; }
      public int          WorldSize    { get; private set; }



      public EcsWorldDebugSystem(string worldName = null, NameSettings nameSettings = null) {
         WorldName    = worldName;
         DebugName    = worldName.ToWorldDebugName();
         NameSettings = nameSettings;
      }

      public void PreInit(IEcsSystems systems) {
         InitWorld(systems);

         View      = new WorldView(this, NameSettings);
         WorldSize = World.GetWorldSize();
         World.AddEventListener(this);

         ActiveDebugSystems.Register(this);

         InitEntities();
      }



      public void Run(IEcsSystems systems) => View.Refresh();

      public void OnEntityChanged(int   entity, short poolId, bool added) { }
      public void OnEntityCreated(int   e) => View.GetEntityView(e).Activate();
      public void OnEntityDestroyed(int e) => View.GetEntityView(e).Deactivate();

      public void OnWorldResized(int newSize) => View.Resize(WorldSize = newSize);

      public void OnWorldDestroyed(EcsWorld world) {
         View.Destroy();
         World.RemoveEventListener(this);
         ActiveDebugSystems.Unregister(this);
      }

      public void OnFilterCreated(EcsFilter filter) { }



      private void InitWorld(IEcsSystems systems) => World = systems.GetWorld(WorldName) ?? throw new Exception($"Cant find required world! ({WorldName})");
      private void InitEntities()                 => World.ForeachEntity(OnEntityCreated);
   }
}

#endif