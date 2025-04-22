using System;

namespace Packages.LeoEcs.LeoEcsDebug.Runtime.Extensions.Type {
   public static class GetCleanNameExt {
      private const string _GENERIC_SYMBOL = "`";


      public static string GetCleanName(this System.Type type) {
         if (!type.IsGenericType)
            return type.Name;

         int genericIndex = type.Name.LastIndexOf(_GENERIC_SYMBOL, StringComparison.Ordinal);

         string cleanName = genericIndex == -1
            ? type.Name
            : type.Name[..genericIndex];

         return cleanName;
      }
   }
}