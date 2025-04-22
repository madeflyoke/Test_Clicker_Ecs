#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Packages.LeoEcs.LeoEcsDebug.Runtime.Attributes;
using UnityEditor;

namespace Packages.LeoEcs.LeoEcsDebug.Runtime.PackedEntity {
   public static class ComponentsWithPackedEntities {
      private static readonly HashSet<Type> _Components = new();

      static ComponentsWithPackedEntities() {
         foreach (FieldInfo fieldInfo in TypeCache.GetFieldsWithAttribute<PackedEntityAttribute>()) {
            _Components.Add(fieldInfo.ReflectedType);
         }
      }

      public static bool Contains(Type component) => _Components.Contains(component);
   }
}
#endif