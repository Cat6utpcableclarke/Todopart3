// Services/ToDoService.cs

using System.Diagnostics;
using System.Text.Json;

namespace To_do_part_3.Services;

public class ToDoService
{
    private readonly HttpClient _httpClient;

    public ToDoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ToDo>> FetchActiveToDosAsync(string userId, string status)
    {
        var url = $"{Constants.URL}{Constants.GET_TODO}?status={status}&user_id={userId}";
        var todos = new List<ToDo>();

        try
        {
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var json = await Task.Run(() =>
                JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(content));

            if (response.IsSuccessStatusCode && json != null &&
                json.TryGetValue("data", out var dataElement) && dataElement.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in dataElement.EnumerateObject())
                {
                    var item = property.Value;

                    todos.Add(new ToDo(
                        item.GetProperty("item_id").GetInt32(),
                        item.GetProperty("item_name").GetString() ?? "",
                        item.GetProperty("item_description").GetString() ?? "",
                        item.GetProperty("status").GetString() ?? "",
                        item.GetProperty("user_id").GetInt32(),
                        DateTime.ParseExact(item.GetProperty("dateTime_created").GetString(), "yyyy-MM-dd HH:mm:ss", null)
                    ));
                }
            }

            return todos;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fetch error: {ex.Message}");
            return todos; // Return empty list on failure
        }
    }
}
