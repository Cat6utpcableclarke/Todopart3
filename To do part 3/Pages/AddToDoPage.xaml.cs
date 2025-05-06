using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace To_do_part_3;

public partial class AddToDoPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    public Action ReloadPage { get; set; }
    public AddToDoPage()
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }

    private async void OnClickReturn(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void Add_Todo_Clicked(object sender, EventArgs e)
    {
        var title = Task_title.Text;
        var description = Task_description.Text;
        var userId = await SecureStorage.GetAsync("user_id");
        var todo_data = new
        {
            item_name = title,
            item_description = description,
            user_id = userId
        };
        Debug.Write(todo_data);
        var jsonContent = new StringContent(JsonSerializer.Serialize(todo_data), Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpClient.PostAsync($"{Constants.URL}{Constants.ADD_TODO}", jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Todo Successfully Added");
                await DisplayAlert("Success", responseJson["message"].ToString(), "OK");

                ReloadPage?.Invoke();
                await Navigation.PopModalAsync();

            }
            else
            {
                Debug.WriteLine("Todo was not added");
                await DisplayAlert("Error", responseJson["message"].ToString(), "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in todoadd: {ex.Message}");
            await DisplayAlert("Error", "An error occured. Please try again.", ex.Message.ToString(), "OK");
        }
    }
}
