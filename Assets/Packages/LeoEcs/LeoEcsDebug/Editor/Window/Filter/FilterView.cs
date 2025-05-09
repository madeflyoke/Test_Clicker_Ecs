﻿using System;
using System.Collections.Generic;
using Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.UIElement;
using Packages.LeoEcs.LeoEcsDebug.Editor.Search;
using Packages.LeoEcs.LeoEcsDebug.Editor.Window.Style;
using UnityEngine;
using UnityEngine.UIElements;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Window.Filter {
   public class FilterView : VisualElement {
      public const string FILTER_CL              = "filter";
      public const string FILTER_BTN_CL          = "filter__btn";
      public const string FILTER_BTN_ADD_TAG_CL  = "filter__btn_add-tag";
      public const string FILTER_BTN_CLEAR_CL    = "filter__btn_clear";
      public const string FILTER_BTN_DISABLED_CL = "filter__btn_disabled";

      private readonly Filter _filter;

      private readonly Dictionary<Type, FilterTagView> _tagViews;

      private Button _addTagBtn;
      private Button _clearBtn;



      public FilterView(Filter filter) {
         _tagViews = new Dictionary<Type, FilterTagView>(capacity: 4);

         _filter             =  filter;
         _filter.OnAddTag    += OnAddFilterTag;
         _filter.OnRemoveTag += OnRemoveFilterTag;

         CreateElements();
         AddElements();
         InitElements();
      }

      public void Reset() {
         ClearFilter();

         _filter.OnAddTag    -= OnAddFilterTag;
         _filter.OnRemoveTag -= OnRemoveFilterTag;
      }



      private void CreateElements() {
         _addTagBtn = new Button(OpenComponentsMenu) { text = Icons.TextPlus };
         _clearBtn  = new Button(ClearFilter) { text        = Icons.TextClose };
      }

      private void AddElements()
         => this
           .AddChild(_addTagBtn)
           .AddChild(_clearBtn);

      private void InitElements() {
         AddToClassList(FILTER_CL);

         _addTagBtn.AddToClassList(FILTER_BTN_CL);
         _addTagBtn.AddToClassList(FILTER_BTN_ADD_TAG_CL);
         _clearBtn.AddToClassList(FILTER_BTN_CL);
         _clearBtn.AddToClassList(FILTER_BTN_CLEAR_CL);

         HideClearButton();
      }



      private void OnAddFilterTag(Type component) {
         _tagViews.Add(component, CreateTagView(component, FilterMethod.Include));
         ShowClearButton();
      }

      private void OnRemoveFilterTag(Type component) {
         Remove(_tagViews[component]);

         if (_filter.Tags.Count <= 0)
            HideClearButton();
      }



      private void ShowClearButton() => _clearBtn.RemoveFromClassList(FILTER_BTN_DISABLED_CL);

      private void HideClearButton() => _clearBtn.AddToClassList(FILTER_BTN_DISABLED_CL);



      private void OpenComponentsMenu() {
         if (!Application.isPlaying)
            return;

         ComponentsSearchWindow.OpenFor(
            _filter.DebugSystem.World,
            _filter.AddTag
         );
      }

      private void ClearFilter() {
         _filter.Clear();
         _tagViews.Clear();
      }


      private FilterTagView CreateTagView(Type componentType, FilterMethod filterMethod) {
         var newView = new FilterTagView(componentType, filterMethod);
         int index   = Mathf.Max(_tagViews.Count, b: 0);

         newView.clicked += () => _filter.RemoveTag(componentType);
         
         newView.AddToClassList(FILTER_BTN_CL);

         Insert(index, newView);
         return newView;
      }
   }
}