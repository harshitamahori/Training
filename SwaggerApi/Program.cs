using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string apiBaseUrl = "https://petstore.swagger.io/v2/pet";

    static async Task Main()
    {
        while (true)
        {
            Console.WriteLine("\n Pet Store CLI ");
            Console.WriteLine("1. Add a new pet");
            Console.WriteLine("2. Fetch pet by ID");
            Console.WriteLine("3. Get available pets");
            Console.WriteLine("4. Update pet");
            Console.WriteLine("5. Delete pet");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await AddPet();
                    break;
                case "2":
                    await GetPetById();
                    break;
                case "3":
                    await GetAvailablePets();
                    break;
                case "4":
                    await UpdatePet();
                    break;
                case "5":
                    await DeletePet();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid choice! Try again.");
                    break;
            }
        }
    }

    static async Task AddPet()
    {
        Console.Write("\nEnter Pet ID: ");
        int petId = int.Parse(Console.ReadLine());

        Console.Write("Enter Pet Name: ");
        string? name = Console.ReadLine();

        var pet = new
        {
            id = petId,
            category = new { id = 1, name = "Dog" },
            name = name,
            photoUrls = new[] { "https://example.com/dog.jpg" },
            tags = new[] { new { id = 1, name = "Cute" } },
            status = "available"
        };

        string jsonPet = JsonSerializer.Serialize(pet);
        var content = new StringContent(jsonPet, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(apiBaseUrl, content);
        string result = await response.Content.ReadAsStringAsync();

        Console.WriteLine("\n Pet Added:");
        Console.WriteLine(FormatJson(result));
    }

    static async Task GetPetById()
    {
        Console.Write("\nEnter Pet ID to fetch: ");
        string? petId = Console.ReadLine();

        HttpResponseMessage response = await client.GetAsync($"{apiBaseUrl}/{petId}");
        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("\n📜 Pet Details:");
            Console.WriteLine(FormatJson(result));
        }
        else
        {
            Console.WriteLine("\n❌ Pet not found!");
        }
    }

    static async Task GetAvailablePets()
    {
        HttpResponseMessage response = await client.GetAsync($"{apiBaseUrl}/findByStatus?status=available");
        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("\n📋 Available Pets:");
            Console.WriteLine(FormatJson(result));
        }
        else
        {
            Console.WriteLine("\n❌ No available pets found!");
        }
    }

    static async Task UpdatePet()
    {
        Console.Write("\nEnter Pet ID to update: ");
        int petId = int.Parse(Console.ReadLine());

        Console.Write("Enter New Pet Name: ");
        string? newName = Console.ReadLine();

        var pet = new
        {
            id = petId,
            category = new { id = 1, name = "Dog" },
            name = newName,
            photoUrls = new[] { "https://example.com/dog.jpg" },
            tags = new[] { new { id = 1, name = "Cute" } },
            status = "available"
        };

        string jsonPet = JsonSerializer.Serialize(pet);
        var content = new StringContent(jsonPet, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PutAsync(apiBaseUrl, content);
        string result = await response.Content.ReadAsStringAsync();

        Console.WriteLine("\nPet Updated:");
        Console.WriteLine(FormatJson(result));
    }

    static async Task DeletePet()
    {
        Console.Write("\nEnter Pet ID to delete: ");
        string? petId = Console.ReadLine();

        HttpResponseMessage response = await client.DeleteAsync($"{apiBaseUrl}/{petId}");
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("\n Pet deleted successfully!");
        }
        else
        {
            Console.WriteLine("\n Error deleting pet!");
        }
    }

    static string FormatJson(string json)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
        return JsonSerializer.Serialize(jsonElement, options);
    }
}
