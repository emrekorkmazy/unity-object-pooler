using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPooling
{
    [CreateAssetMenu(fileName = "NewObjectPoolConfig", menuName = "Object Pooler/Object Pool Config")]
    internal class ObjectPoolConfig : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initialPoolSize = 10;
        [SerializeField] private bool canExpand = true;

        public GameObject Prefab => prefab;
        public int InitialPoolSize => initialPoolSize;
        public bool CanExpand => canExpand;

    }
}