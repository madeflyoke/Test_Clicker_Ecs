﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Leopotam.EcsLite;
using Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.EntityView;
using Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.Style.Border;
using Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.Style.Spacing;
using Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.Style.Text;
using Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.UIElement;
using Packages.LeoEcs.LeoEcsDebug.Runtime.Attributes;
using Packages.LeoEcs.LeoEcsDebug.Runtime.PackedEntity;
using Packages.LeoEcs.LeoEcsDebug.Runtime.View;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.Style.StyleConsts;

namespace Packages.LeoEcs.LeoEcsDebug.Editor {
   [CustomPropertyDrawer(typeof(ComponentView), useForChildren: true)]
   public class ComponentDrawer : PropertyDrawer {
      private const string _COMPONENT_NAME_FIELD = nameof(ComponentView.componentName);
      private const string _COMPONENT_FIELD      = nameof(ComponentView.Component);

      private Box           _root;
      private VisualElement _header;
      private Label         _label;
      private VisualElement _main;
      private VisualElement _fields;

      private SerializedProperty _property;

      private ComponentView _target;

      private object Component     => _target.Component;
      private Type   ComponentType => _target.ComponentType;



      public override VisualElement CreatePropertyGUI(SerializedProperty property) {
         _property = property;
         _target   = (ComponentView)property.GetUnderlyingValue();

         CreateElements();
         StructureElements();
         InitElements();
         return _root;
      }



      private void CreateElements() {
         _root   = new Box();
         _header = new VisualElement();
         _label  = new Label();
         _main   = new VisualElement();
         _fields = new VisualElement();
      }

      private void StructureElements() {
         _root
           .AddChild(_header.AddChild(_label))
           .AddChild(_main.AddChild(_fields.AddChildPropertiesOf(ComponentProperty())));
         AddPackedEntities();
      }

      private void InitElements() {
         _root
           .style
           .Margin(hor: 0, REM_025)
           .Padding(REM_05)
           .BorderRadius(REM_05);

         _label
           .SetText(ComponentName())
           .style
           .FontStyle(FontStyle.Bold);

         _main.style.paddingLeft = REM;
      }



      private void AddPackedEntities() {
         if (!ComponentsWithPackedEntities.Contains(ComponentType))
            return;

         foreach (FieldInfo field in ComponentType.GetFields()) {
            if (!field.HasAttribute<PackedEntityAttribute>())
               continue;

            string title      = field.HumanName();
            object fieldValue = field.GetValueOptimized(Component);

            switch (fieldValue) {
               case EcsPackedEntity packed:
                  _main.Add(
                     new EntityField(
                        packed,
                        _target.World,
                        view => {
                           field.SetValueOptimized(Component, view.PackedEntity());
                           _target.SetValue(Component);
                        },
                        title
                     )
                  );
                  break;
               case EcsPackedEntityWithWorld packedWithWorld:
                  _main.Add(
                     new EntityField(
                        packedWithWorld,
                        view => {
                           field.SetValueOptimized(Component, view.PackedEntityWithWorld());
                           _target.SetValue(Component);
                        },
                        title
                     )
                  );
                  break;
               case IList collection:
                  var entitiesContainer = new Foldout { text = title };

                  switch (collection) {
                     case IList<EcsPackedEntity> packedEntities:
                        for (var i = 0; i < packedEntities.Count; i++) {
                           IList<EcsPackedEntity> entities = packedEntities;
                           int                    index    = i;

                           entitiesContainer.Add(
                              new EntityField(
                                 packedEntities[i],
                                 _target.World,
                                 view => entities[index] = view.PackedEntity(),
                                 title
                              )
                           );
                        }

                        break;
                     case IList<EcsPackedEntityWithWorld> packedWithWorldEntities:
                        for (var i = 0; i < packedWithWorldEntities.Count; i++) {
                           IList<EcsPackedEntityWithWorld> entities = packedWithWorldEntities;
                           int                             index    = i;

                           entitiesContainer.Add(
                              new EntityField(
                                 packedWithWorldEntities[i],
                                 view => entities[index] = view.PackedEntityWithWorld(),
                                 title
                              )
                           );
                        }

                        break;
                  }

                  _main.Add(entitiesContainer);
                  break;
            }
         }
      }



      private string             ComponentName()         => ComponentNameProperty().stringValue;
      private SerializedProperty ComponentNameProperty() => _property.FindPropertyRelative(_COMPONENT_NAME_FIELD);
      private SerializedProperty ComponentProperty()     => _property.FindPropertyRelative(_COMPONENT_FIELD);
   }
}