﻿using UnityEngine.UIElements;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.Style.Border {
   public static class BorderColorExt {
      public static IStyle BorderColor(
         this IStyle style,
         StyleColor  top,
         StyleColor  bot,
         StyleColor  left,
         StyleColor  right
      ) {
         style.borderTopColor    = top;
         style.borderBottomColor = bot;
         style.borderLeftColor   = left;
         style.borderRightColor  = right;

         return style;
      }

      public static IStyle BorderColor(
         this IStyle style,
         StyleColor  color
      ) {
         style.BorderColor(color, color, color, color);

         return style;
      }
   }
}