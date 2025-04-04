namespace To_do_part_3;

public partial class EditToDo : ContentPage
{
	public EditToDo()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);

    }
    private async void OnClickReturn(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}