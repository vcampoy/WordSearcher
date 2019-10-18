using Unity;

namespace WordSearcher.Infrastructure.Implementation
{
    public static class IoCHelper
    {
        private static IUnityContainer _container;

        public static IUnityContainer GetContainer()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
            }

            return _container;
        }
    }
}
