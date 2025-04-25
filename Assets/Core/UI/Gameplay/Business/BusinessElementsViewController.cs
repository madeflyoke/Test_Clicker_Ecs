using System.Collections.Generic;
using Core.Business.Enums;
using Core.Factory;
using UnityEngine;

namespace Core.UI.Gameplay.Business
{
    public class BusinessElementsViewController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        private Dictionary<BusinessType, int> _businessEntities;
        
        private Dictionary<BusinessElementView, int> _currentElementsViews;
        private BusinessViewFactory _viewFactory;
        
        public void Initialize(BusinessViewFactory factory, Dictionary<BusinessType, int> entities)
        {
            _businessEntities = entities;
            _viewFactory = factory;
            _currentElementsViews = new Dictionary<BusinessElementView, int>();
            
            foreach (var kvp in _businessEntities)
            {
                var instance = _viewFactory.ConstructNew(_container);
                _viewFactory.ApplyEntityToView(instance, kvp.Value);
                _currentElementsViews.Add(instance, kvp.Value);
            }
        }
    }
}
