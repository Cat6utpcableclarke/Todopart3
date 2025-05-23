using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace To_do_part_3;

public partial class EditCompleted : ContentPage
{
    private ToDo _toDo;
    private readonly HttpClient _httpClient = new HttpClient();
    public EditCompleted(ToDo toDo)
	{
		InitializeComponent();
        _toDo = toDo;
        BindingContext = _toDo;

    }
    private async void OnClickReturn(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void Delete(object sender, EventArgs e) 
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

            }
            else
            {
                Debug.WriteLine($"Error: {responseJson["message"].ToString}");

            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        await Navigation.PopModalAsync();
    }
    

    private async void Incomplete(object sender, EventArgs e) 
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

        var url = $"{Constants.URL}{Constants.CHANGE_TODOSTAT}";
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
        await Navigation.PopModalAsync();
    }

    private async void Update(object sender, EventArgs e) 
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