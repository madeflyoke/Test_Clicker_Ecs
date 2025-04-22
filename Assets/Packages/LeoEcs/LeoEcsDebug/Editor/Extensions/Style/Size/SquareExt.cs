using UnityEngine.UIElements;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.Style.Size {
   public static class SquareExt {
      public static IStyle Square(
         this IStyle style,
         StyleLength size
      ) {
         style.width  = size;
         style.height = size;

         return style;
      }
   }
}