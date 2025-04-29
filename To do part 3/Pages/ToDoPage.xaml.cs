
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
namespace To_do_part_3;

public partial class ToDoPage : ContentPage
{
	private ObservableCollection<ToDo> toDoList = new ObservableCollection<ToDo>();
    private readonly HttpClient _httpClient = new HttpClient();

    public ToDoPage()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        //toDoList.Add(new ToDo (1,"Task1","task1", false));
        //toDoList.Add(new ToDo (2,"Task2","task2", false));
        //toDoList.Add(new ToDo (3,"Task3", "task3", false));
        //ToDos.ItemsSource = toDoList;
    }


    private async void Edit_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new EditToDo(), true);
        Debug.WriteLine("Edit Clicked");

    }

	private void Delete_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Delete Clicked");
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Add Clicked");
        //await Shell.Current.GoToAsync("///AddToDo");
        await Navigation.PushModalAsync(new AddToDoPage(), true);
    }

    private void Done_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Done Clicked");
        var button = (Button)sender;
        var toDo = (ToDo)button.CommandParameter;
        //Debug.WriteLine(toDo.Task);
    }

    private async void Get_ToDo()
    {
        var user_id = await SecureStorage.GetAsync("user_id");


        var url = $"{Constants.URL}{Constants.GET_TODO}?status=active&user_id={user_id}";
        
        try
        {
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response JSON: {responseContent}");

            var responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success","Successfully retrieved data", "OK");
            }
            else
            {
                await DisplayAlert("Error", responseJson["message"].ToString(), "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}", ex);
            await DisplayAlert("Error", "An error occurred. Please try again.", ex.Message.ToString(), "OK");
        }


    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Get_ToDo();
    }
}