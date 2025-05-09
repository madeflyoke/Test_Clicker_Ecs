﻿using System;
using Leopotam.EcsLite;
using Packages.LeoEcs.LeoEcsDebug.Runtime;
using UnityEditor;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Window {
   public abstract class BaseEcsDebugWindow : EditorWindow, IEcsWorldEventListener {
      public EcsWorldDebugSystem ActiveSystem { get; private set; }

      protected bool Open;



      private void OnEnable() {
         EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
         EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

         ActiveDebugSystems.OnRegister -= OnRegisterSystem;
         ActiveDebugSystems.OnRegister += OnRegisterSystem;

         ActiveDebugSystems.OnUnregister -= OnUnregisterSystem;
         ActiveDebugSystems.OnUnregister += OnUnregisterSystem;
      }

      private void OnDisable() {
         EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

         ActiveDebugSystems.OnRegister -= OnRegisterSystem;

         ActiveDebugSystems.OnUnregister -= OnUnregisterSystem;

         Open = false;
      }

      private void CreateGUI() {
         Open = true;

         CreateElements();
         StructureElements();
         InitElements();
      }



      protected abstract void OnRegisterSystem(EcsWorldDebugSystem   system);
      protected abstract void OnUnregisterSystem(EcsWorldDebugSystem system);



      public virtual void OnEntityCreated(int   e)                                { }
      public virtual void OnEntityChanged(int   entity, short poolId, bool added) { }
      public virtual void OnEntityDestroyed(int entity) { }

      public virtual void OnWorldResized(int        newSize) { }
      public virtual void OnWorldDestroyed(EcsWorld world)   { }

      public virtual void OnFilterCreated(EcsFilter filter) { }



      protected abstract void CreateElements();
      protected abstract void StructureElements();
      protected abstract void InitElements();


      protected abstract void InitInspector();
      protected abstract void ResetInspector();



      protected void ChangeWorld(EcsWorldDebugSystem system) {
         if (ActiveDebugSystems.TryGet(system.WorldName, out EcsWorldDebugSystem debugSystem))
            SetActiveWorldDebugSystem(debugSystem);
         else
            throw new Exception($"Can't find System relative to `{system}` world!");
      }


      private void SetActiveWorldDebugSystem(EcsWorldDebugSystem newActiveDebugSystem) {
         if (ActiveSystem == newActiveDebugSystem)
            return;

         ResetActiveSystem();

         ActiveSystem = newActiveDebugSystem;

         InitActiveSystem();
      }


      private void InitActiveSystem() {
         if (ActiveSystem == null)
            return;

         ActiveSystem.World.AddEventListener(this);
         InitInspector();
      }

      private void ResetActiveSystem() {
         if (ActiveSystem == null)
            return;

         ActiveSystem.World.RemoveEventListener(this);
         ResetInspector();
      }



      private void OnPlayModeStateChanged(PlayModeStateChange state) {
         switch (state) {
            case PlayModeStateChange.EnteredEditMode:
            case PlayModeStateChange.ExitingEditMode:
            case PlayModeStateChange.EnteredPlayMode:
               break;
            case PlayModeStateChange.ExitingPlayMode:
               ResetInspector();
               break;
            default:
               throw new ArgumentOutOfRangeException(nameof(state), state, message: null);
         }
      }
   }
}