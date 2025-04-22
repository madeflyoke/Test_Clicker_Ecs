using Leopotam.EcsLite;
using UnityEngine;

namespace Packages.LeoEcs.EcsConversion
{
    [RequireComponent(typeof(ComponentsContainer))]
    public abstract class BaseConverter : MonoBehaviour
    {
        public abstract void Convert(EcsPackedEntityWithWorld entityWithWorld);
    }
}