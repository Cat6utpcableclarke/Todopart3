using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using To_do_part_3.Services;
namespace To_do_part_3;

public partial class CompletedToDo : ContentPage
{
    private ObservableCollection<ToDo> toDoList = new ObservableCollection<ToDo>();
    private readonly ToDoService _toDoService = new ToDoService(new HttpClient());
    private readonly HttpClient _httpClient = new HttpClient();
    public CompletedToDo()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }


    private async void Delete_Clicked(object sender, EventArgs e)
    {

        var button = (ImageButton)sender;
        var todeleteToDo = (ToDo)button.CommandParameter;
        var URL = $"{Constants.URL}{Constants.DELETE}?item_id={todeleteToDo.ItemId}";

        try
        {
            var response = await _httpClient.DeleteAsync(URL);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseContent);

            var status = responseJson["status"].GetInt32();

            if (status == 200)
            {
                await LoadToDosAsync();  // Wait for list refresh before hiding loading
            }
            else
            {
                Debug.WriteLine($"Error: {responseJson["message"].ToString()}");
                await DisplayAlert("Error", responseJson["message"].ToString(), "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error", "skibiddi", "OK");
            await DisplayAlert("Error", "An error occurred. Please try again.", "OK");
        }
    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
         var toDo = (ToDo)((ImageButton)sender).CommandParameter;
        Debug.WriteLine("Edit Clicked");

        await Navigation.PushModalAsync(new EditCompleted(toDo), true);
    }

    private async Task LoadToDosAsync()
    {
        LoadingOverlay.IsVisible = true;
        await Task.Delay(50); // Let loading animation start

        var userId = await SecureStorage.GetAsync("user_id");
        var fetchedTodos = await _toDoService.FetchActiveToDosAsync(userId, "inactive");

        toDoList = new ObservableCollection<ToDo>(fetchedTodos);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            CToDos.ItemsSource = toDoList;
        });


        LoadingOverlay.IsVisible = false;
    }

    private async void Undone_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Done Clicked");
        var button = (ImageButton)sender;
        var toDo = (ToDo)button.CommandParameter;
        Debug.WriteLine(toDo.ItemId);

        var data = new
        {
            status = "active",
            item_id = toDo.ItemId
        };

        var url = $"{Constants.URL}{Constants.CHANGE_TODOSTAT}?status=inactive&item_id={toDo.ItemId}";
        var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpClient.PostAsync(url, jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response: {responseContent}");
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseContent);

            if (responseJson != null && responseJson.ContainsKey("status"))
            {

                var status = responseJson["status"].GetInt32();
                if (status == 200)
                {
                    //Do Nothing
                }
                else
                {
                    await DisplayAlert("Error", "An unexpected status code was returned.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}");
            await DisplayAlert("Error", "An error occurred. Please try again.", "OK");
        }
        finally
        {
            await LoadToDosAsync();
        }
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadToDosAsync();
    }

}