using System.Collections.Generic;
using UnityEngine;

namespace Core.UI.Gameplay.Business
{
    public class BusinessElementsHolder : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        private List<BusinessElementView> _currentElements;

        public void Add(List<BusinessElementView> elements)
        {
            _currentElements = elements;
        }
    }
}
