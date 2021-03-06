using Xamarin.Forms;
using XamDogsOut.Models;
using XamDogsOut.Services;
using XamDogsOut.Services.FirestoreData;

namespace XamDogsOut
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            DependencyService.Register<IDataProvider<Dog>, DogTable>();
            DependencyService.Register<IDataProvider<Profile>, ProfileTable>();
            DependencyService.Register<IDataProvider<Request>, RequestTable>();
            DependencyService.Register<IDataProvider<AcceptedRequest>, AcceptedRequestTable>();


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
