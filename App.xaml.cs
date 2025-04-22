using System;
using System.Windows;
using ChanceForHappiness.Services;

namespace ChanceForHappiness
{
    public partial class App : Application
    {
        public static NavigationService? NavigationService { get; private set; }
        public static DataService? DataService { get; private set; }
        public static LoggingService? LoggingService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LoggingService = new LoggingService();
            LoggingService.Log("Application starting...");

            DataService = new DataService();
            NavigationService = new NavigationService();

            LoggingService.Log("Application initialized successfully");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LoggingService.Log("Application shutting down");
            base.OnExit(e);
        }
    }
}