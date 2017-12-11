using System.Windows;

namespace RazorRockstars.CompiledViews.WpfHost
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            new AppHost()
                .Init()
                .Start("http://*:3333/");
        }
    }
}
