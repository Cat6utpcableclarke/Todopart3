using System.Diagnostics;
using System.Threading.Tasks;

namespace To_do_part_3;

public partial class SignUp : ContentPage
{
    public SignUp()
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }

    private async void SignUpButtonClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Sign Up Clicked");
        await Shell.Current.GoToAsync("//ToDoPage");


    }

    private async void SignInButtonClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Sign In Clicked");
        await Shell.Current.GoToAsync("//SignIn");
    }
}
