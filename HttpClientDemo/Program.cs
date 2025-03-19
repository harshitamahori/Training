// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System.Net;
using System.Net.Http.Json; // for Http Requests
using System.Text;  // for encoding 
using System.Text.Json; //for serialization and deserialisation

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
        await GetPaginatedData(sharedClient);
        await PostAsync(sharedClient);
        await PostAsJsonAsync(sharedClient);
        await PutAsync(sharedClient);
        await PutAsJsonAsync(sharedClient);
        await PatchAsync(sharedClient);
        await DeleteAsync(sharedClient);
        await HeadAsync(sharedClient);


        Console.WriteLine("All tasks completed.");
    }

    static async Task GetAsync(HttpClient httpClient)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync("todos/3");
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");

        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetAsync: {ex.Message}");
        }
       
    }

    //Define a ToDo class of type record that is used to store details and has these 4 properties
    //to get structured data
    public record class Todo(int? UserId = null, int? Id = null, string? Title = null, bool? Completed = null);

    static async Task GetFromJsonAsync(HttpClient httpClient)
    {
        try
        {

            var todos = await httpClient.GetFromJsonAsync<List<Todo>>("todos?userId=1&completed=false");
            Console.WriteLine("GET https://jsonplaceholder.typicode.com/todos?userId=1&completed=false HTTP/1.1");
            todos?.ForEach(Console.WriteLine);
            Console.WriteLine();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetFromJsinAsync: {ex.Message}");
        }
        
    }
    static async Task GetPaginatedData(HttpClient httpClient)
    {
        try
        {
            int page = 1; // Start from the first page
            int pageSize = 5; // Fetch 5 items per page
            bool hasMoreData = true;

            while (hasMoreData)
            {
                string url = $"todos?_page={page}&_limit={pageSize}"; // JSONPlaceholder supports _page & _limit
                var todos = await httpClient.GetFromJsonAsync<List<Todo>>(url);

                if (todos == null || todos.Count == 0)
                {
                    hasMoreData = false; // Stop if no more data
                    break;
                }

                Console.WriteLine($"Page {page} Data:");
                todos.ForEach(Console.WriteLine);
                Console.WriteLine();

                page++; // Move to the next page
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetFromJsonAsync: {ex.Message}");
        }
    }


    static async Task PostAsync(HttpClient httpClient)
    {
        try
        {
            //StringContent is helper class that wraps string data in an Http request - use when you have to send json data
            using StringContent jsonContent = new(
            JsonSerializer.Serialize(new { userId = 77, id = 1, title = "write code sample", completed = false }),
            Encoding.UTF8,
            "application/json");
            //using - to despose off the resources properly after usage. To free up system storage
            using HttpResponseMessage response = await httpClient.PostAsync("todos", jsonContent);
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in PostAsync: {ex.Message}");
        }
        
        
    }

    static async Task PostAsJsonAsync(HttpClient httpClient)
    {
        try
        {
            //HttpResponseMessage - Represents the HTTP response received from the server.
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "todos", new Todo(UserId: 9, Id: 99, Title: "Show extensions", Completed: false));

            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var todo = await response.Content.ReadFromJsonAsync<Todo>();
            Console.WriteLine($"{todo}\n");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in PostAsJsonAsyn: {ex.Message}");
        }
        
    }

    static async Task PutAsync(HttpClient httpClient)
    {
        try
        {
            //send data in request body has 3 parameters - content, encosing and media type
            using StringContent jsonContent = new(
            JsonSerializer.Serialize(new { userId = 1, id = 1, title = "foo bar", completed = false }),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await httpClient.PutAsync("todos/1", jsonContent);
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in PutAsync: {ex.Message}");
        }
        
    }
    static async Task PutAsJsonAsync(HttpClient httpClient)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync("todos/5",
            new Todo(Title: "partially update todo", Completed: true));

            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var todo = await response.Content.ReadFromJsonAsync<Todo>();
            Console.WriteLine($"{todo}\n");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in  PutAsJsonAsync: {ex.Message}");
        }
        
    }
    static async Task PatchAsync(HttpClient httpClient)
    {
        try
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(
            new
            {
                completed = true
            }),
            Encoding.UTF8,
            "application/json");
            //to store response - status code etc .
            using HttpResponseMessage response = await httpClient.PatchAsync("todos/1", jsonContent);

            response.EnsureSuccessStatusCode()
                .WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in PatchAsync: {ex.Message}");
        }
        
    }
    static async Task DeleteAsync(HttpClient httpClient)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync("todos/1");

            response.EnsureSuccessStatusCode()
                .WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch(HttpRequestException ex) when (ex is { StatusCode: HttpStatusCode.NotFound })
        {
            Console.WriteLine($"Not Found: {ex.Message}");
        }
       
    }
    static async Task HeadAsync(HttpClient httpClient)
    {
        try
        {
            using HttpRequestMessage request = new(HttpMethod.Head, "https://jsonplaceholder.typicode.com");
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode().WriteRequestToConsole();
            foreach (var header in response.Headers)
            {
                Console.WriteLine($"{header.Key}:{string.Join(", ", header.Value)}");
            }
            Console.WriteLine();
        }
        catch (HttpRequestException ex) when (ex is { StatusCode: HttpStatusCode.NotFound })
        {
            // Handle 404
            Console.WriteLine($"Not found: {ex.Message}");
        }

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

