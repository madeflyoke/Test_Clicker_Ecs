using UnityEngine.UIElements;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.Style.Text {
   public static class FontSizeExt {
      public static IStyle FontSize(this IStyle style, StyleLength value) {
         style.fontSize = value;
         return style;
      }
   }
}