﻿using Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.UIElement;
using Packages.LeoEcs.LeoEcsDebug.Runtime;
using UnityEngine.UIElements;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Window.Entity {
   public sealed class ListEntity : VisualElement {
      private const string _MAIN_CL            = "entity_row";
      public const  string MAIN_LABEL_CL      = "entity__label";
      public const  string MAIN_TAG_CL        = "entity__tag";
      public const  string MAIN_INDEX_CL      = "entity__index";
      public const  string MAIN_COMPONENTS_CL = "entity__components";

      private Label _label;



      public ListEntity() {
         CreateElements();
         AddElements();
         InitElements();
      }



      public void Setup(int e, EcsWorldDebugSystem system)
         => _label.SetText(
            system
              .View
              .GetEntityView(e)
              .name
         );



      public void Activate() => style.display = DisplayStyle.Flex;

      public void Deactivate() => style.display = DisplayStyle.None;



      private void CreateElements() => _label = new Label();

      private void AddElements() => this.AddChild(_label);

      private void InitElements() => AddToClassList(_MAIN_CL);
   }
}