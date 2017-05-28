using System;
using System.Net.Http;
using DryIoc;

namespace LightsOut
{
    public static class RegistratorExtensions
    {
        public static IRegistrator SetupViewModels(this IRegistrator registrator)
        {
            registrator.Register<ILightsOutGameViewModel, LightsOutGameViewModel>();
            registrator.Register<ISwitchViewModel, SwitchViewModel>();
            registrator.Register<ILevelsLoader, LevelsLoader>();
            registrator.RegisterDelegate<Func<HttpClient>>(r => () => new HttpClient());
            registrator.RegisterDelegate<Func<ISwitchViewModel>>(r => () => r.Resolve<ISwitchViewModel>());
            return registrator;
        }
    }
}