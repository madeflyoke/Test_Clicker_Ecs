// ----------------------------------------------------------------------------
// The MIT-Red License
// Copyright (c) 2012-2024 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Extensions {
    public static class EditorExtensions {
        public static string GetCleanGenericTypeName (Type type) {
            if (!type.IsGenericType) {
                return type.Name;
            }
            var constraints = "";
            foreach (var constraint in type.GetGenericArguments ()) {
                constraints += constraints.Length > 0 ? $", {GetCleanGenericTypeName (constraint)}" : constraint.Name;
            }
            var genericIndex = type.Name.LastIndexOf ("`", StringComparison.Ordinal);
            var typeName = genericIndex == -1
                ? type.Name
                : type.Name.Substring (0, genericIndex);
            return $"{typeName}<{constraints}>";
        }
    }
    
    public sealed class EcsSystemsDebugView : MonoBehaviour {
        [NonSerialized] public List<string> PreInitSystems;
        [NonSerialized] public List<string> InitSystems;
        [NonSerialized] public List<string> RunSystems;
        [NonSerialized] public List<string> PostRunSystems;
        [NonSerialized] public List<string> DestroySystems;
        [NonSerialized] public List<string> PostDestroySystems;
    }
}
