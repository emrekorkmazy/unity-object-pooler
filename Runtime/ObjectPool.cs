using UnityEngine;
namespace ObjectPooling
{
    public interface ObjectPool<T> where T : MonoBehaviour, PoolableObject
    {
        public T Spawn(Vector3 position, Quaternion rotation);
        public void Despawn(T obj);
    }
}