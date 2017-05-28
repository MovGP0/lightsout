using System;
using DryIoc;
using DryIoc.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;

namespace LightsOut
{
    public static class DependencyInjectionSetup
    {
        public static void InitializeServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => new DryIocServiceLocator(Container));
        }

        public static IContainer Container => ContainerFactory.Value;
        private static Lazy<IContainer> ContainerFactory => new Lazy<IContainer>(SetupContainer);

        private static IContainer SetupContainer()
        {
            var container = new Container();
            SetupRegistrator(container);
            return container;
        }

        private static void SetupRegistrator(IRegistrator registrator)
        {
            registrator.SetupViewModels();
        }
    }
}
