﻿using System;
using System.Collections.Generic;
using System.Linq;
using Packages.LeoEcs.LeoEcsDebug.Runtime;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Window.Filter {
   public class Filter {
      public event Action<Type> OnAddTag;
      public event Action<Type> OnRemoveTag;

      public EcsWorldDebugSystem         DebugSystem { get; private set; }
      public Dictionary<Type, FilterTag> Tags        { get; }



      public Filter() {
         Tags = new Dictionary<Type, FilterTag>(capacity: 4);
      }

      public void Init(EcsWorldDebugSystem debugSystem) {
         DebugSystem = debugSystem;

         foreach (FilterTag tag in Tags.Values) {
            tag.SetWorld(DebugSystem.World);
         }
      }

      public void Clear() {
         foreach (Type component in Tags.Keys)
            RemoveTag(component, removeViewOnly: true);

         Tags.Clear();
      }

      public void Reset() {
         DebugSystem = null;
         Clear();
      }



      public bool AddTag(Type component) {
         if (Tags.TryGetValue(component, out FilterTag tag)) {
            UnityEngine.Debug.Log($"Tag: [ {component.Name} ] is already added with method: [ {tag.Method} ]!");
            return false;
         }

         FilterMethod method = FilterMethod.Include;

         Tags.Add(
            component,
            new FilterTag(
               component,
               method,
               DebugSystem.World
            )
         );

         OnAddTag?.Invoke(component);
         return true;
      }

      public void RemoveTag(Type component, bool removeViewOnly = false) {
         if (!Tags.ContainsKey(component)) {
            UnityEngine.Debug.Log($"Tag: [ {component.Name} ] not found! (internal)");
            return;
         }

         if (!removeViewOnly)
            Tags.Remove(component);

         OnRemoveTag?.Invoke(component);
      }



      public bool IsEmpty() => Tags.Count <= 0;

      public bool Has(int e)
         => Tags
           .Values
           .Select(
               tag => tag.Method switch {
                  FilterMethod.Include => tag.Pool.Has(e),
                  FilterMethod.Exclude => !tag.Pool.Has(e),
                  var _                => throw new ArgumentOutOfRangeException()
               }
            )
           .All(compatible => compatible);
   }
}