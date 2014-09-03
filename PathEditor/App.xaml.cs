using System.Diagnostics;
using System.Security.Principal;
using System.Windows;

namespace PathEditor
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (IsAdministrator() == false)
            {
                // Restart program and run as admin
                var exeName = Process.GetCurrentProcess().MainModule.FileName;
                var startInfo = new ProcessStartInfo(exeName) {Verb = "runas"};
                Process.Start(startInfo);
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
