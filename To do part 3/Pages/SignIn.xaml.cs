using System.Diagnostics;

namespace To_do_part_3;

public partial class SignIn : ContentPage
{
	public SignIn()
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this, false);
    }


	public async void SignInClicked(object sender, EventArgs e) {

		Debug.WriteLine("CLicked");

        await Shell.Current.GoToAsync("//ToDoPage");
    }


	public async void SignUpClicked(object sender, EventArgs e) {
		Debug.WriteLine("Clicked");
		await Shell.Current.GoToAsync("//SignUp");
	
	}
}