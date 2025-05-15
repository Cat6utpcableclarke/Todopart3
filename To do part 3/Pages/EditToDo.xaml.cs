using System.Diagnostics;
using System.Text.Json;
using System.Text;


namespace To_do_part_3;

public partial class EditToDo : ContentPage
{
    private ToDo _toDo;
    private readonly HttpClient _httpClient = new HttpClient();
    public EditToDo(ToDo todo)
	{
        InitializeComponent();
        _toDo = todo;
        BindingContext = _toDo;
	}
    private async void OnClickReturn(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void UpdateTask(object sender, EventArgs e)
    {
        Debug.WriteLine($"Updated Title: {_toDo.ItemName}");
        Debug.WriteLine($"Updated Description: {_toDo.ItemDescription}");
        var todo_data = new
        {
            item_name = _toDo.ItemName,
            item_description = _toDo.ItemDescription,
            item_id = _toDo.ItemId

        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(todo_data), Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpClient.PostAsync($"{Constants.URL}{Constants.UPDATE}", jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Todo Successfully Added");
                await DisplayAlert("Success", responseJson["message"].ToString(), "OK");

  
                await Navigation.PopModalAsync();

            }
            else
            {
                Debug.WriteLine("Task was not updated");
                await DisplayAlert("Error", responseJson["message"].ToString(), "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in ToDoUpdate: {ex.Message}");
            await DisplayAlert("Error", "An error occured. Please try again.", ex.Message.ToString(), "OK");
        }

    }


}