using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace To_do_part_3;

public partial class CompletedToDo : ContentPage
{
    private ObservableCollection<ToDo> toDoList = new ObservableCollection<ToDo>();
    private readonly HttpClient _httpClient = new HttpClient();
    public CompletedToDo()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }


    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var todeleteToDo = (ToDo)button.CommandParameter;
        Debug.WriteLine(todeleteToDo.ItemId);
        var URL = $"{Constants.URL}{Constants.DELETE}?item_id={todeleteToDo.ItemId}";
        Debug.WriteLine("Delete Clicked");

        try
        {
            var response = await _httpClient.DeleteAsync(URL);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseContent);

            var status = responseJson["status"].GetInt32();

            if (status == 200)
            {
                Debug.WriteLine($"{responseJson["message"].ToString}");
                ReloadPage();
            }
            else
            {
                Debug.WriteLine($"Error: {responseJson["message"].ToString}");
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error", "skibiddi", "OK");
        }

    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
         var toDo = (ToDo)((Button)sender).CommandParameter;
        Debug.WriteLine("Edit Clicked");

        await Navigation.PushModalAsync(new EditCompleted(toDo), true);
    }

    private async Task Get_ToDo()
    {
        var user_id = await SecureStorage.GetAsync("user_id");

        var url = $"{Constants.URL}{Constants.GET_TODO}?status=inactive&user_id={user_id}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response JSON for the actual todo: {responseContent}");

            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseContent);

            if (response.IsSuccessStatusCode)
            {
                if (responseJson.TryGetValue("data", out var dataElement) && dataElement.ValueKind == JsonValueKind.Object)
                {
                    toDoList.Clear();
                    foreach (var property in dataElement.EnumerateObject())
                    {
                        var item = property.Value;

                        int id = item.GetProperty("item_id").GetInt32();
                        string title = item.GetProperty("item_name").GetString() ?? string.Empty;
                        string description = item.GetProperty("item_description").GetString() ?? string.Empty;
                        string status = item.GetProperty("status").GetString() ?? string.Empty;
                        int userID = item.GetProperty("user_id").GetInt32();
                        DateTime timeM = DateTime.ParseExact(item.GetProperty("dateTime_created").GetString(), "yyyy-MM-dd HH:mm:ss", null);


                        Debug.WriteLine($"Items in JSON: {id}, {title}, {description}, {status}, {userID}, {timeM}");

                        var todo = (new ToDo(id, title, description, status, userID, timeM));
                        Debug.WriteLine("Inside the todo: ", todo);

                        toDoList.Add(todo);
                    }

                    CToDos.ItemsSource = toDoList;
                }
                else
                {
                    Debug.WriteLine("Error in getting data");
                }
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

    private async void Undone_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Done Clicked");
        var button = (Button)sender;
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
        ReloadPage();

        //Debug.WriteLine(toDo.Task);
    }
    private async void ReloadPage()
    {
        Debug.WriteLine("Reloading ToDoPage...");
       await  Get_ToDo(); // Refresh the data
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Get_ToDo();
    }

}