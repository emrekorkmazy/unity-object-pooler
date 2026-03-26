using UnityEngine;
using System.Collections.Generic;

namespace ObjectPooling
{
    internal class ObjectPoolImpl<T> : ObjectPool<T> where T : MonoBehaviour, PoolableObject
    {
        private T prefab;
        private Queue<T> pool = new Queue<T>();
        private List<T> activeObjects = new List<T>();
        private Transform parent;
        private bool canExpand;
        private int size = 0;

        internal ObjectPoolImpl(T prefab, int initialSize, bool canExpand, Transform parent = null)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.canExpand = canExpand;

            for (int i = 0; i < initialSize; i++)
            {
                InstantiateNewObject();
            }
        }

        public T Spawn(Vector3 position, Quaternion rotation)
        {
            T obj;
            if (pool.Count == 0)
            {
                if (canExpand)
                    obj = InstantiateNewObject();
                else
                    Despawn(activeObjects[0]); // Despawn the oldest active object to make room
            }
            obj = pool.Dequeue();
            activeObjects.Add(obj);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.gameObject.SetActive(true);
            obj.OnSpawn();
            return obj;
        }

        public void Despawn(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.ResetState();
            pool.Enqueue(obj);
            activeObjects.Remove(obj);
        }

        private T InstantiateNewObject()
        {
            Debug.Log($"Instantiating new object of type {typeof(T).Name}. Current pool size: {size}");
            T obj = GameObject.Instantiate(prefab, parent);
            obj.name = prefab.name + "_" + size; // Optional: Name the object for easier debugging
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
            size++;
            return obj;
        }
    }
}