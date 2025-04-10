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

        // Replace these with actual user input values from the UI
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

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Sign In Successful");
                await DisplayAlert("Success", responseJson["message"].ToString(), "OK");
                await Shell.Current.GoToAsync("//ToDoPage");
            }
            else
            {
                Debug.WriteLine($"Sign In Failed: {response.StatusCode}");
                await DisplayAlert("Error", responseJson["message"].ToString(), "OK");
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