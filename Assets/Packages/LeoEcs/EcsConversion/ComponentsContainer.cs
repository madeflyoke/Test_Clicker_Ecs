using UnityEngine;

namespace Packages.LeoEcs.EcsConversion
{
    [DisallowMultipleComponent]
    public class ComponentsContainer : MonoBehaviour
    {
        [SerializeField] private BaseConverter[] _converters;
        public BaseConverter[] Converters => _converters;
        public bool DestroyAfterConversion => true;

        internal void GetConverters()
        {
            _converters = GetComponents<BaseConverter>();
        }
    }
}