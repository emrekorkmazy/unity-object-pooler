using System;
using UnityEngine;

namespace ObjectPooling
{
    public interface PoolableObject
    {
        void OnSpawn();
        void ResetState();
    }
}