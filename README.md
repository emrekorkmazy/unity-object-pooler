# Unity Object Pooler Usage Guide

This guide explains how to use the Unity Object Pooler system.

## Installation

1. Import the package into your Unity project.
2. Create the `Assets/Resources/ObjectPooler` folder (if it doesn't exist).

## Configuration

Create an `ObjectPoolConfig` ScriptableObject for each pool:

1. Right-click in the `Assets/Resources/ObjectPooler` folder.
2. Select `Create > Object Pooler > Object Pool Config`.
3. Edit the config:
   - **Prefab**: The GameObject prefab to use in the pool. This prefab must have one component that implements the `PoolableObject` interface.
   - **Initial Pool Size**: The initial pool size.
   - **Can Expand**: Whether the pool can grow beyond the initial size when all objects are in use. If disabled, the oldest active object will be despawned to make room for new spawns.

## PoolableObject Interface

Objects to be used in the pool must implement the `PoolableObject` interface:

```csharp
using ObjectPooling;

public class Bullet : MonoBehaviour, PoolableObject
{
    public Rigidbody rb;

    public void OnSpawn()
    {
        // Called when the object is spawned from the pool
        // Perform any additional initialization here
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
    }

    public void ResetState()
    {
        // Reset the object to its state before spawning
        // Example: reset velocity, position

    }
}
```

- **OnSpawned**: An action that is invoked each time the object is spawned from the pool. Use this to initialize or activate the object (e.g., start animations, enable components).
- **ResetState()**: This method should reset all state of the component to its initial state before spawning. This includes velocity, position, rotation, health, timers, and any other variables that may have changed during use. Ensure the object is in a clean state ready for reuse.

## Usage

### Without VContainer

Call `ObjectPoolFactory.Initialize()` during scene loading (e.g., in the loading screen). If not called, it initializes automatically on first use.

```csharp
using ObjectPooling;

public class BulletSpawner : MonoBehaviour
{
    private ObjectPool<Bullet> bulletPool;

    void Awake()
    {
        bulletPool = ObjectPoolFactory.GetPool<Bullet>();
    }

    public void SpawnBullet()
    {
        bulletPool.Spawn(Vector3.zero, Quaternion.identity);
    }
}
```

### With VContainer 1.14.0+

If VContainer is available, use dependency injection. Note: When using VContainer, the `ObjectPoolFactory` class is marked as internal, so `Initialize()` is not accessible and attempting to call it will result in a compile error.

```csharp
using ObjectPooling;
using VContainer;

public class BulletSpawner : MonoBehaviour
{
    private ObjectPool<Bullet> bulletPool;

    [Inject]
    private void Construct(PoolProvider<Bullet> poolProvider)
    {
        bulletPool = poolProvider.Pool;
    }

    public void SpawnBullet()
    {
        bulletPool.Spawn(Vector3.zero, Quaternion.identity);
    }
}
```

## API Reference

### ObjectPool<T>

- `T Spawn(Vector3 position, Quaternion rotation)`: Retrieves an object from the pool and spawns it at the specified position.
- `void Despawn(T obj)`: Returns the object to the pool.

### ObjectPoolFactory

- `static void Initialize()`: Initializes the pools.
- `static ObjectPool<T> GetPool<T>()`: Returns the pool for the specified type.

### PoolProvider<T> (with VContainer)

- `ObjectPool<T> Pool`: The injected pool.

## Best Practices

- **Avoid using Unity's Start() method**: In components implementing `PoolableObject`, do not rely on Unity's `Start()` method for initialization, as it is only called once during the object's lifetime and not when the object is spawned from the pool. Instead, use the `OnSpawned` action to perform initialization each time the object is reused.
- **Thoroughly reset state in ResetState()**: Ensure that `ResetState()` resets every aspect of the component's state to prevent carry-over from previous uses, which could lead to bugs or unexpected behavior.

## Troubleshooting

- "No pool configuration found" error: Ensure `ObjectPoolConfig` is under `Assets/Resources/ObjectPooler`.
- Missing `PoolableObject` implementation on prefab: Implement this interface on all pool objects.
