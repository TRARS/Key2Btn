using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Key2Btn.Base.Helper.Extensions
{
    public static class ServiceExtensions
    {
        //public static IServiceCollection AddFormFactory<TService>(this IServiceCollection services) where TService : class
        //{
        //    return services.AddTransient<TService>()
        //                   .AddSingleton<Func<TService>>(sp => () => sp.GetService<TService>()!)
        //                   .AddSingleton<IAbstractFactory<TService>, AbstractFactory<TService>>();
        //}

        public static IServiceCollection AddFormFactory<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services) where TService : class
                                                                                                                                                                                                     where TImplementation : class, TService
        {
            return services.AddTransient(typeof(TService), typeof(TImplementation))
                           .AddSingleton<Func<TService>>(sp => () => sp.GetService<TService>()!)
                           .AddSingleton<IAbstractFactory<TService>, AbstractFactory<TService>>();
        }

        //public static IServiceCollection AddFormFactory<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
        //{
        //    return services.AddTransient<TService>()
        //                   .AddSingleton<Func<TService>>(sp => () => sp.GetService<TService>()!)
        //                   .AddSingleton<IAbstractFactory<TService>, AbstractFactory<TService>>()
        //                   .AddSingleton(typeof(TService), implementationFactory);
        //}

        //public static IServiceCollection AddFormFactory<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
        //                                                                                                                                                                                                                                                     where TImplementation : class, TService
        //{
        //    return services.AddTransient(typeof(TService), typeof(TImplementation))
        //                   .AddSingleton<Func<TService>>(sp => () => sp.GetService<TService>()!)
        //                   .AddSingleton<IAbstractFactory<TService>, AbstractFactory<TService>>()
        //                   .AddSingleton(typeof(TService), implementationFactory);
        //}
    }
}
