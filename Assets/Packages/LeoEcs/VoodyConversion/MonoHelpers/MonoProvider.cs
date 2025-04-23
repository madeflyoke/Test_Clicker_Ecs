using Leopotam.EcsLite;
using Sirenix.Serialization;
using UnityEngine;

namespace Packages.LeoEcs.VoodyConversion.MonoHelpers
{
    public abstract class MonoProvider<T> : BaseMonoProvider, IConvertToEntity where T : struct
    {
        [OdinSerialize] protected T value;
        
        void IConvertToEntity.Convert(int entity, EcsWorld world)
        {
            var pool = world.GetPool<T>();
            if (pool.Has(entity))
            {
                pool.Del(entity);
            }

            pool.Add(entity) = value;
        }
    }
}
