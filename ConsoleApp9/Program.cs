using System;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

public class Program
{
    private static string PerformRequest(string url)
    {
        //Console application for working with remote REST API
        //- ability to send a request to a remote API
        //- method API returns 2 lists - Products and Categories
        //- the application should output the received response to the console
        
        
        var client = new HttpClient(); 

        var response = client.GetAsync(url).Result; 
        if (response.IsSuccessStatusCode) 
        {
            return response.Content.ReadAsStringAsync().Result; 
        }

        return null;
    }

    // Perform GET request to server and display response to user in next style
    // ----------------------------------
    // |    Product    |    Category    |
    // ----------------------------------
    // |    Laptop     |    Computers   |
    //       
    // |    Bread      |      Food      |
    // ----------------------------------
    private static void SendRequest()
    {
        var jsonContent = PerformRequest("http://tester.consimple.pro");
        if (!string.IsNullOrEmpty(jsonContent))
        {
            Response response = JsonSerializer.Deserialize<Response>(jsonContent);

            var divider = new String('-', 47); // Table divider like "---------------------"
            var tableFormat = "| {0,-20} | {1,-20} |"; // Format to provide same length of table content

            // Print Table header
            Console.WriteLine(divider);
            Console.WriteLine(String.Format(tableFormat, "Product", "Category"));
            Console.WriteLine(divider); 

            // Print table content from GET response
            foreach (Product product in response.Products)
            {
                var categoryName = "-";
                foreach (Category category in response.Categories)
                {
                    if (category.Id == product.CategoryId)
                    {
                        categoryName = category.Name;
                        break;
                    }
                }

                var lineStr = String.Format(tableFormat, product.Name, categoryName);
                Console.WriteLine(lineStr);
            }

            // Print Table Footer
            Console.WriteLine(divider);
        }
        else
        {
            Console.WriteLine("Error: No server response");
        }
    }

    public static void Main()
    {
        while (true)
        {
            // Show help commands
            Console.WriteLine("Please, enter command:");
            Console.WriteLine("s - send GET request to server");
            Console.WriteLine("q - exit");

            // Get User input
            string userInput = Console.ReadLine();

            // Process user input
            if (!string.IsNullOrEmpty(userInput))
            {
                switch (userInput[0])
                {
                    case 'q':
                        Console.WriteLine("Exiting...");
                        return;
                    case 's':
                        SendRequest();
                        break;
                }
            }
        }
    }
}

// Class for JSON parsing
public class Response
{
    public IList<Product> Products { get; set; }
    public IList<Category> Categories { get; set; }
}

// Class for JSON parsing
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
}

// Class for JSON parsing
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
}
