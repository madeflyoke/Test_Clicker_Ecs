#if UNITY_EDITOR
namespace Packages.LeoEcs.LeoEcsDebug.Runtime.Name {
   public class NameSettings {
      public bool   BakeComponents { get; }
      public string Format         { get; }

      public NameSettings(bool bakeComponents = false, string format = null) { // "X8"
         BakeComponents = bakeComponents;
         Format         = format;
      }
   }
}
#endif