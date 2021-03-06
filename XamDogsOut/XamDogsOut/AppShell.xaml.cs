using Xamarin.Forms;
using XamDogsOut.Views;

namespace XamDogsOut
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(RequestInfoPage), typeof(RequestInfoPage));
            Routing.RegisterRoute(nameof(DetailsRequestPage), typeof(DetailsRequestPage));



        }
    }
}
