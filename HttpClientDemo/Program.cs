// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

class Program
{
    //make instance of HttpClient for reusability
    private static readonly HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
    };

    static async Task Main()
    {
        Console.WriteLine("Starting HTTP Client Example...");

        await GetAsync(sharedClient);
        await GetFromJsonAsync(sharedClient);
        await PostAsync(sharedClient);
        await PostAsJsonAsync(sharedClient);
        await PutAsync(sharedClient);
        await PutAsJsonAsync(sharedClient);
        await PatchAsync(sharedClient);
        await DeleteAsync(sharedClient);

        Console.WriteLine("All tasks completed.");
    }

    static async Task GetAsync(HttpClient httpClient)
    {
        using HttpResponseMessage response = await httpClient.GetAsync("todos/3");
        response.EnsureSuccessStatusCode().WriteRequestToConsole();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");
    }

    //Define a ToDo class of type record that is used to store details and has these 4 properties
    //to get structured data
    public record class Todo(int? UserId = null, int? Id = null, string? Title = null, bool? Completed = null);

    static async Task GetFromJsonAsync(HttpClient httpClient)
    {
        var todos = await httpClient.GetFromJsonAsync<List<Todo>>("todos?userId=1&completed=false");

        Console.WriteLine("GET https://jsonplaceholder.typicode.com/todos?userId=1&completed=false HTTP/1.1");
        todos?.ForEach(Console.WriteLine);
        Console.WriteLine();
    }

    static async Task PostAsync(HttpClient httpClient)
    {
        //StringContent is helper class that wraps string data in an Http request - use when you have to send json data
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new { userId = 77, id = 1, title = "write code sample", completed = false }),
            Encoding.UTF8,
            "application/json");

        using HttpResponseMessage response = await httpClient.PostAsync("todos", jsonContent);
        response.EnsureSuccessStatusCode().WriteRequestToConsole();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");
    }

    static async Task PostAsJsonAsync(HttpClient httpClient)
    {
        //HttpResponseMessage - Represents the HTTP response received from the server.
        using HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "todos", new Todo(UserId: 9, Id: 99, Title: "Show extensions", Completed: false));

        response.EnsureSuccessStatusCode().WriteRequestToConsole();

        var todo = await response.Content.ReadFromJsonAsync<Todo>();
        Console.WriteLine($"{todo}\n");
    }

    static async Task PutAsync(HttpClient httpClient)
    {
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new { userId = 1, id = 1, title = "foo bar", completed = false }),
            Encoding.UTF8,
            "application/json");

        using HttpResponseMessage response = await httpClient.PutAsync("todos/1", jsonContent);
        response.EnsureSuccessStatusCode().WriteRequestToConsole();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");
    }
    static async Task PutAsJsonAsync(HttpClient httpClient)
    {
        using HttpResponseMessage response = await httpClient.PutAsJsonAsync("todos/5",
            new Todo(Title: "partially update todo", Completed: true));

        response.EnsureSuccessStatusCode().WriteRequestToConsole();

        var todo = await response.Content.ReadFromJsonAsync<Todo>();
        Console.WriteLine($"{todo}\n");
    }
    static async Task PatchAsync(HttpClient httpClient)
    {
        using StringContent jsonContent = new(JsonSerializer.Serialize(
            new
            {
                completed = true
            }),
            Encoding.UTF8,
            "application/json");
        //to store response - status code etc .
        using HttpResponseMessage response = await httpClient.PatchAsync("todos/1",jsonContent);

        response.EnsureSuccessStatusCode()
            .WriteRequestToConsole();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");
    }
    static async Task DeleteAsync(HttpClient httpClient)
    {
        using HttpResponseMessage response = await httpClient.DeleteAsync("todos/1");

        response.EnsureSuccessStatusCode()
            .WriteRequestToConsole();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");
    }
}

static class HttpResponseMessageExtensions
{
    internal static void WriteRequestToConsole(this HttpResponseMessage response)
    {
        if (response is null) return;
        var request = response.RequestMessage;
        Console.Write($"{request?.Method} ");
        Console.Write($"{request?.RequestUri} ");
        Console.WriteLine($"HTTP/{request?.Version}");
    }
}

