using System.Collections.Generic;
using System.Linq;
using Core.Business.Enums;
using Core.Factory;
using Core.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Gameplay.Business
{
    public class BusinessElementsViewController : MonoBehaviour
    {
        [SerializeField] private RectTransform _viewRect;
        [SerializeField] private VerticalLayoutGroup _container;
        private Dictionary<BusinessType, int> _businessEntities;
        
        private Dictionary<BusinessElementView, int> _currentElementsViews;
        private List<BusinessElementView> _elements;
        private BusinessViewFactory _viewFactory;
        private bool _initialized;
        
        public void Initialize(BusinessViewFactory factory, Dictionary<BusinessType, int> entities)
        {
            _businessEntities = entities;
            _viewFactory = factory;
            _currentElementsViews = new Dictionary<BusinessElementView, int>();
            
            foreach (var kvp in _businessEntities)
            {
                var instance = _viewFactory.Construct(_container.transform);
                _viewFactory.ApplyEntityToView(instance, kvp.Value);
                _currentElementsViews.Add(instance, kvp.Value);
            }

            _elements = _currentElementsViews.Keys.ToList();
            
            DisableLayout();
            _initialized = true;
        }

        private void DisableLayout() //apply dynamic scroll pool system maybe
        {
            var containerRt = _container.transform as RectTransform;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            var containerSize = containerRt.rect;

            _container.TryGetComponent<ContentSizeFitter>(out var sizeFitter);
            sizeFitter.enabled = false;
            _container.enabled = false;
            
            containerRt.rect.Set(containerSize.position.x,containerSize.position.y,containerSize.width,containerSize.height);
        }
        
        private void Update()
        {
            if (!_initialized)
                return;
            foreach (var element in _elements)
            {
                var rt = (RectTransform)element.transform;
                
                if(ScrollViewVisibility.IsElementVisible(rt, _viewRect))
                {
                    element.gameObject.SetActive(true);
                }
                else
                {
                    element.gameObject.SetActive(false);
                }
            }
        }
    }
}
