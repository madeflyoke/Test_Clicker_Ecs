using UnityEngine.UIElements;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.UIElement {
   public static class SetTextExt {
      public static TextElement SetText(this TextElement label, string text) {
         label.text = text;
         return label;
      }
   }
}