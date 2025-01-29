using NotificationCenter.Domain.Entities;
using NotificationCenter.Domain.Enums;
using System.Net.Http.Json;

class Program
{
    static async Task Main(string[] args)
    {
        // Initialize HttpClient
        using var httpClient = new HttpClient();
        // Adjust the base address for your API
        httpClient.BaseAddress = new Uri("https://localhost:44320/");

        Console.WriteLine("=== Notification Event Generator ===");
        Console.WriteLine("Press Enter to create a new test event or type 'E'/'exit' to quit...");

        while (true)
        {
            // Wait for user input
            var input = Console.ReadLine();

            // If user types 'E' or 'exit', break out of the loop
            if (input?.Equals("E", StringComparison.OrdinalIgnoreCase) == true ||
                input?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
            {
                Console.WriteLine("Exiting...");
                break;
            }

            try
            {
                // Create a new NotificationEvent
                var newEvent = new NotificationEvent
                {
                    EventName = $"New Test Event - {DateTime.UtcNow}",
                    Channel = Channel.Web,
                    TargetGroup = TargetGroup.All,
                    RequestId = null,
                    CertificateId = null
                };

                // Post the event to the API
                var response = await httpClient.PostAsJsonAsync("api/notifications/addnotificationevent", newEvent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Notification event created and processed successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to create notification event: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error details: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press Enter to add another test notification event, or type 'E'/'exit' to quit...");
        }

        Console.WriteLine("Program has ended. Press any key to close.");
        Console.ReadKey();
    }
}