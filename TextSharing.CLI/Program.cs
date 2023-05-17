using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Models;

namespace CommandLineApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var baseUrl = "http://localhost:5169/api/snippets/";

            Console.WriteLine("Welcome to the Text Snippet Command Line App!");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Add new snippet");
                Console.WriteLine("2. View snippet");
                Console.WriteLine("3. Exit");
                Console.Write("Option: ");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await AddSnippet(httpClient, baseUrl);
                        break;
                    case "2":
                        await ViewSnippet(httpClient, baseUrl);
                        break;
                    case "3":
                        Console.WriteLine("Exiting the application...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static async Task AddSnippet(HttpClient httpClient, string baseUrl)
        {
            Console.WriteLine("Enter the text of the snippet (press Enter on an empty line to send):");

            var lines = new List<string>();
            string line;
            while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
            {
                lines.Add(line);
            }

            var text = string.Join(Environment.NewLine, lines);

            var snippet = new { Text = text };
            var response = await httpClient.PostAsJsonAsync(baseUrl, snippet);

            if (response.IsSuccessStatusCode)
            {
                var slug = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Snippet added successfully! Slug: {slug}");
            }
            else
            {
                Console.WriteLine("Failed to add snippet.");
            }
        }

        static async Task ViewSnippet(HttpClient httpClient, string baseUrl)
        {
            Console.Write("Enter the slug of the snippet: ");
            var slug = Console.ReadLine();

            var response = await httpClient.GetAsync($"{baseUrl}{slug}");

            if (response.IsSuccessStatusCode)
            {
                var snippet = await response.Content.ReadFromJsonAsync<Snippet>();
                Console.WriteLine($"Snippet: {snippet?.Text}");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Snippet not found.");
            }
            else
            {
                Console.WriteLine("Failed to retrieve snippet.");
            }
        }
    }
}
