using System.Diagnostics;

namespace To_do_part_3;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this, false);
		GetUserDetail();

    }

	private async Task GetUserDetail()
	{
		string fName = await SecureStorage.GetAsync("fname");
		string lName = await SecureStorage.GetAsync("lname");

		Debug.WriteLine(fName, lName);
		if (fName == null || lName == null) {
			fName = "";
			lName = "";
		}
		Name.Text = fName +" "+ lName;
	}
	private async void EditProfile(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new EditProfile(), true);
	}

    private async void SignOut(object sender, EventArgs e)
    {
		SecureStorage.Remove("fname");
		SecureStorage.Remove("lname");
		await Shell.Current.GoToAsync("//SignIn");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetUserDetail();
    }
}