namespace To_do_part_3;

public partial class EditProfile : ContentPage
{
	public EditProfile()
	{
		InitializeComponent();
	}
    private async void OnClickReturn(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}