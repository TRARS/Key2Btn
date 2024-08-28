using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper;
using Key2Btn.Base.Helper.Extensions;
using Key2Btn.MainView;
using Key2Btn.MainView.Interfaces;
using Key2Btn.MainView.Services;
using Key2Btn.MainView.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace Key2Btn
{
    public partial class App : Application
    {
        private static IHost AppHost { get; set; } = GetHostBuilder().Build();

        public App()
        {
            //使SelectionTextBrush生效
            AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();
            AppHost.Services.GetRequiredService<MainWindow>().ShowEx();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            base.OnExit(e);
        }

        private static IHostBuilder GetHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                       .ConfigureServices(sc =>
                       {
                           // MainView
                           sc.AddSingleton<MainWindow>(sp => new()
                           {
                               DataContext = sp.GetRequiredService<IAbstractFactory<IMainWindow_viewmodel>>().Create(),
                               SizeToContent = SizeToContent.WidthAndHeight,
                           });

                           // MainVM
                           sc.AddFormFactory<IMainWindow_viewmodel, MainWindow_viewmodel>();
                           sc.AddFormFactory<IuTitleBarVM, uTitleBarVM>();
                           sc.AddFormFactory<IuRainbowLineVM, uRainbowLineVM>();
                           sc.AddFormFactory<IuClientVM, uClientVM>();
                           sc.AddFormFactory<ITaskbarIconVM, TaskbarIconVM>();

                           // Service
                           sc.AddSingleton<IMessageBoxService, MessageBoxService>();
                           sc.AddSingleton<IProfileService, ProfileService>();
                           sc.AddSingleton<IStringFactoryService, StringFactoryService>();
                           sc.AddSingleton<IActionExecutor, ActionExecutor>();

                           // MenuVM
                           sc.AddSingleton<GridBtnMenuVM>();
                       });
        }

        public static T GetRequiredService<T>() where T : notnull
        {
            return AppHost.Services.GetRequiredService<T>();
        }
    }
}
