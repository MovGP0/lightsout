using Microsoft.Practices.ServiceLocation;

namespace LightsOut
{
    public static class ResourceLocator
    {
        static ResourceLocator()
        {
            DependencyInjectionSetup.InitializeServiceLocator();
        }

        public static ILightsOutGameViewModel LightsOutGameViewModel => ServiceLocator.Current.GetInstance<ILightsOutGameViewModel>();
    }
}