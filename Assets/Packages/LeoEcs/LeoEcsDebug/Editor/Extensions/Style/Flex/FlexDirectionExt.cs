﻿using UnityEngine.UIElements;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.Style.Flex {
   public static class FlexDirectionExt {
      public static IStyle FlexRow(this IStyle style, bool reverse = false) {
         style.flexDirection = !reverse
            ? FlexDirection.Row
            : FlexDirection.RowReverse;
         return style;
      }

      public static IStyle FlexColumn(this IStyle style, bool reverse = false) {
         style.flexDirection = !reverse
            ? FlexDirection.Column
            : FlexDirection.ColumnReverse;
         return style;
      }
   }
}