using Xamarin.Forms;
using XamDogsOut.Models;
using XamDogsOut.Services;

namespace XamDogsOut
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            DependencyService.Register<IDataProvider<Dog>, DogTable>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
