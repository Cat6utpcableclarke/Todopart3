using System.Net.Http;
using System.Text.Json;
using System.Diagnostics;
namespace To_do_part_3;

public partial class SignIn : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();

    public SignIn()
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }

    public async void SignInClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Sign In Clicked");

        var email = Email.Text; // Assuming EmailEntry is the Entry for email
        var password = Password.Text; // Assuming PasswordEntry is the Entry for password

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Please enter both email and password.", "OK");
            return;
        }

        var url = $"{Constants.URL}/signin_action.php?email={email}&password={password}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            Debug.WriteLine($"HTTP Status Code: {response.StatusCode}");

            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize into Dictionary<string, JsonElement>
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseContent);

            Debug.WriteLine("Response JSON:");
            Debug.WriteLine(JsonSerializer.Serialize(responseJson, new JsonSerializerOptions { WriteIndented = true }));

            if (responseJson != null && responseJson.ContainsKey("status"))
            {
                // Use GetInt32() to extract the status
                var status = responseJson["status"].GetInt32();

                if (status == 200)
                {
                    Debug.WriteLine("Sign In Successful");
                    await DisplayAlert("Success", responseJson["message"].GetString(), "OK");

                    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseJson["data"].ToString());
                    var userId = data["id"].ToString();
                    await SecureStorage.SetAsync("user_id", userId);
                    await Shell.Current.GoToAsync("//ToDoPage");
                }
                else if (status == 400)
                {
                    Debug.WriteLine("Sign In Failed: Email or password is incorrect.");
                    await DisplayAlert("Error", responseJson["message"].GetString(), "OK");
                }
                else
                {
                    Debug.WriteLine("Unexpected Status Code");
                    await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
                }
            }
            else
            {
                Debug.WriteLine("Unexpected Response Format");
                await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}");
            await DisplayAlert("Error", "An error occurred. Please try again.", "OK");
        }
    }


    public async void SignUpClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Sign Up Clicked");
        await Navigation.PushAsync(new SignUp());
    }
}