using UnityEngine.UIElements;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.UIElement {
   public static class AddChildExt {
      public static T AddChild<T, TChild>(this T root, TChild visualElement)
         where T : VisualElement
         where TChild : VisualElement {
         root.Add(visualElement);
         return root;
      }
   }
}