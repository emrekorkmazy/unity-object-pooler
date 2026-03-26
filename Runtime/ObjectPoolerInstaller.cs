#if VCONTAINER_1_14_0_OR_NEWER
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ObjectPooling
{
    public class ObjectPoolerInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            ObjectPoolFactory.Initialize();
            builder.Register(typeof(PoolProviderImpl<>), Lifetime.Singleton)
           .As(typeof(PoolProvider<>));
        }
    }
}
#endif