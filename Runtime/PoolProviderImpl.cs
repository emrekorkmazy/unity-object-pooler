#if VCONTAINER_1_14_0_OR_NEWER
using UnityEngine;
namespace ObjectPooling
{
    internal class PoolProviderImpl<T> : PoolProvider<T> where T : MonoBehaviour, PoolableObject
    {
        public ObjectPool<T> Pool { get; }

        public PoolProviderImpl()
        {
            Pool = ObjectPoolFactory.GetPool<T>();
        }
    }
}
#endif