using System;
using System.Collections.Generic;
using System.Linq;
using Packages.LeoEcs.LeoEcsDebug.Editor.Extensions.UIElement;
using Packages.LeoEcs.LeoEcsDebug.Editor.Utils;
using Packages.LeoEcs.LeoEcsDebug.Runtime;
using Packages.LeoEcs.LeoEcsDebug.Runtime.Extensions.Ecs.World;
using UnityEngine.UIElements;

namespace Packages.LeoEcs.LeoEcsDebug.Editor.Window.Entity {
   public sealed class EntitiesList : VisualElement {
      public const string MAIN_CL         = "entities-list";
      public const string MAIN_CONTENT_CL = "entities-list__content";

      private readonly HashSet<int>  _allEntities;
      private readonly List<int>     _entities;
      private readonly Filter.Filter _filter;

      private readonly ObjectPool<VisualElement> _viewsPool;

      private EcsWorldDebugSystem _debugSystem;

      private ListView _listView;

      private int _selectedEntity = -1;

      public Action<int> OnSelect;
      public Action<int> OnUnselect;



      public EntitiesList(Filter.Filter filter) {
         _filter = filter;

         _allEntities = new HashSet<int>(capacity: 128);
         _entities    = new List<int>(capacity: 128);
         _viewsPool   = new ObjectPool<VisualElement>(() => new ListEntity());

         CreateElements();
         AddElements();
         InitElements();
      }



      public void Setup(EcsWorldDebugSystem debugSystem) {
         _debugSystem = debugSystem;
         AddAllEntities(_debugSystem);
      }

      public void Reset() {
         _allEntities.Clear();
         _entities.Clear();
         _listView.RefreshItems();
      }



      public void Add(int e) => _allEntities.Add(e);

      public void Remove(int e) {
         if (_selectedEntity == e)
            _listView.RemoveFromSelection(_entities.IndexOf(e));

         _allEntities.Remove(e);
      }

      public void Refresh() {
         _entities.Clear();
         _entities.AddRange(
            !_filter.IsEmpty()
               ? _allEntities.Where(_filter.Has)
               : _allEntities
         );
         _entities.Sort();

         _listView.RefreshItems();
      }



      private void CreateElements() => _listView = new ListView();

      private void AddElements() => this.AddChild(_listView);

      private void InitElements() {
         AddToClassList(MAIN_CL);

         _listView.AddToClassList(MAIN_CONTENT_CL);

         _listView.itemsSource = _entities;

         _listView.showBoundCollectionSize    = true;
         _listView.horizontalScrollingEnabled = true;
         
         _listView.selectedIndicesChanged -= SelectIndices;
         _listView.selectedIndicesChanged += SelectIndices;

         _listView.makeItem   = MakeEntity;
         _listView.bindItem   = BindEntity;
         _listView.unbindItem = UnbindEntity;
      }



      private void AddAllEntities(EcsWorldDebugSystem debugSystem) => debugSystem.World.ForeachEntity(Add);



      private void SelectIndices(IEnumerable<int> indices) {
         int[] enumerable = indices as int[] ?? indices.ToArray();


         int curSelectedEntity = enumerable.Length >= 1
            ? _entities[enumerable.First()]
            : -1;


         if (_selectedEntity >= 0)
            OnUnselect?.Invoke(_selectedEntity);

         if (curSelectedEntity >= 0)
            OnSelect?.Invoke(curSelectedEntity);


         _selectedEntity = curSelectedEntity;
      }



      private VisualElement MakeEntity() => _viewsPool.Take();

      private void BindEntity(VisualElement element, int i) {
         var listEntity = (ListEntity)element;
         int e          = _entities[i];
         listEntity.Setup(e, _debugSystem);
      }

      private void UnbindEntity(VisualElement element, int i) => _viewsPool.Return(element);
   }
}