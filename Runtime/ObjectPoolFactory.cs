using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
#if VCONTAINER_1_14_0_OR_NEWER
    internal class ObjectPoolFactory
#else
    public static class ObjectPoolFactory
#endif
    {
        private static Dictionary<Type, ObjectPoolConfig> configLookup = new Dictionary<Type, ObjectPoolConfig>();
        private static Dictionary<Type, object> pools = new Dictionary<Type, object>();
        private static bool isInitialized = false;

        static void ValidateInitialization()
        {
            if (!isInitialized)
            {
                Debug.LogWarning("ObjectPoolFactory not initialized. Initializing now...");
                Initialize();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticFields()
        {
            configLookup.Clear();
            pools.Clear();
            isInitialized = false;
        }

        public static void Initialize()
        {
            if (isInitialized) return;
            var configs = Resources.LoadAll<ObjectPoolConfig>("ObjectPooler");
            foreach (var config in configs)
            {
                Type type = config.Prefab.GetComponent<PoolableObject>().GetType();
                if (!typeof(PoolableObject).IsAssignableFrom(type))
                {
                    Debug.LogError($"Prefab {config.Prefab.name} does not implement PoolableObject interface!");
                    continue;
                }
                configLookup[type] = config;
            }
            isInitialized = true;
        }

        public static ObjectPool<T> GetPool<T>() where T : MonoBehaviour, PoolableObject
        {
            ValidateInitialization();
            ObjectPool<T> pool;
            if (!pools.ContainsKey(typeof(T)))
            {
                ObjectPoolConfig config;
                if (!configLookup.TryGetValue(typeof(T), out config))
                {
                    Debug.LogError($"No pool configuration found for type {typeof(T).Name}! Ensure ObjectPoolConfig is in Resources/ObjectPooler/");
                    return null;
                }
                pool = CreatePool<T>(config);
                pools[typeof(T)] = pool;
            }
            pool = (ObjectPool<T>)pools[typeof(T)];
            return pool;
        }

        private static ObjectPool<T> CreatePool<T>(ObjectPoolConfig config) where T : MonoBehaviour, PoolableObject
        {
            ValidateInitialization();
            if (config.Prefab == null)
                Debug.LogError($"Prefab for type {typeof(T).Name} is not assigned in the pool configuration!");

            ObjectPool<T> pool = new ObjectPoolImpl<T>(config.Prefab.GetComponent<T>(), config.InitialPoolSize, config.CanExpand, new GameObject($"{config.Prefab.name} Pool").transform);
            return pool;
        }
    }
}