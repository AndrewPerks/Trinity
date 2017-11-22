using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Trinity.Authentication;
using Trinity.Helpers;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;
using Trinity.ViewModels;
using Trinity.Views;
using Microsoft.Practices.Unity;

namespace Trinity
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IApplicationSettingService _applicationSettingService;
        DispatcherTimer timer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public Int32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public Int32 dwTime;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _applicationSettingService = ContainerHelper.Container.Resolve<ApplicationSettingService>();

            // Create a custom principal with an anonymous identity at startup
            CustomPrincipal customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

            base.OnStartup(e);

            // Track the period of inactivity
            EventManager.RegisterClassHandler(typeof(Window), Window.PreviewMouseMoveEvent, new MouseEventHandler(OnPreviewMouseMove));
            EventManager.RegisterClassHandler(typeof(Window), Window.PreviewKeyDownEvent, new KeyEventHandler(OnPreviewKeyDown));
            stopWatch.Start();

            //Ticks for every two minutes
            timer.Interval = new TimeSpan(0, 0, 10);

            timer.Tick += timer_Tick;
            timer.Start();

            // Show the login view
            AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationManager());
            IView loginWindow = new Login(viewModel);
            loginWindow.Show();
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            stopWatch.Restart();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            stopWatch.Restart();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            var val = GetLastInputTime();
            var appSetting = _applicationSettingService.GetTimeOutLength();
            var timeoutLength = Convert.ToDouble(appSetting.Value);

            //If the user is idle for configurable amount of seconds disable the current MainWindow
            if (stopWatch.Elapsed.TotalHours >= timeoutLength)
            {
                // Reset user principal and logout
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal != null)
                {
                    customPrincipal.Identity = new AnonymousIdentity();
                }      

                // Open login window and close all other windows
                if (!(Current.MainWindow is Login))
                {
                    AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationManager());
                    var loginWindow = new Login(viewModel);
                    Current.MainWindow = loginWindow;
                    loginWindow.Show();

                    foreach (var window in Current.Windows)
                    {
                        if (!(window is Login))
                        {
                            (window as Window).Close();
                        }     
                    }                                 
                }
            }
        }

        //Returns the period of time the user is idle in seconds
        static int GetLastInputTime()
        {
            int idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            int envTicks = (int)Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                int lastInputTick = lastInputInfo.dwTime;

                idleTime = envTicks - lastInputTick;
            }

            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }

    }
}
