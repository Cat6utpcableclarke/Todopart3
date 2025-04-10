using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
namespace To_do_part_3;

public partial class SignUp : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();

    public SignUp()
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }

    private async void SignUpButtonClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Sign Up Clicked");

        // Replace these with actual user input values from the UI
        var firstName = UserName.Text;
        var lastName = UserName.Text;
        var email = EmailAddress.Text;
        var password = Password.Text;
        var confirmPassword = ConfirmPasswword.Text;

        var signUpData = new
        {
            first_name = firstName,
            last_name = lastName,
            email = email,
            password = password,
            confirm_password = confirmPassword
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(signUpData), Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync($"{Constants.URL}{Constants.SIGNUP}", jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Sign Up Successful");
                await DisplayAlert("Success", responseJson["message"].ToString(), "OK");
                await Shell.Current.GoToAsync("//ToDoPage");
            }
            else
            {
                Debug.WriteLine($"Sign Up Failed: {response.StatusCode}");
                await DisplayAlert("Error", responseJson["message"].ToString(), "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}");
            await DisplayAlert("Error", "An error occurred. Please try again.", "OK");
        }
    }

    private async void SignInButtonClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Sign In Clicked");
        //await Shell.Current.GoToAsync("//SignIn");
        await Navigation.PopAsync();
    }
}