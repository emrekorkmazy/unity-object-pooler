#if VCONTAINER_1_14_0_OR_NEWER
using UnityEngine;

namespace ObjectPooling
{
    public interface PoolProvider<T> where T : MonoBehaviour, PoolableObject
    {
        ObjectPool<T> Pool { get; }
    }
}
#endif