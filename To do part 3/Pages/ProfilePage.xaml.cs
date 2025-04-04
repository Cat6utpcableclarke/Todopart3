namespace To_do_part_3;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this, false);
    }

	private async void EditProfile(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new EditProfile(), true);
	}

}