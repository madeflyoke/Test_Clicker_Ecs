using UnityEngine.UIElements;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.Style {
   public static class PercentsLengthExt {
      public static StyleLength Percents(this float length)
         => new(
            new Length(
               length,
               LengthUnit.Percent
            )
         );

      public static StyleLength Percents(this int length) => ((float)length).Percents();
   }
}