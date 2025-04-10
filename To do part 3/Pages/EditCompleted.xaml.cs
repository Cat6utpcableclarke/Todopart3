namespace To_do_part_3;

public partial class EditCompleted : ContentPage
{
	public EditCompleted()
	{
		InitializeComponent();
	}
    private async void OnClickReturn(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

}