using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        List<string> filePaths = new List<string>();
        for (int i = 1; i <= 10; i++)
        {
            string filePath = $@"C:\Users\Omen\Documents\{i}.json";
            if (File.Exists(filePath))
            {
                filePaths.Add(filePath);
            }
        }

        List<Product> allProducts = new List<Product>();
        foreach (string filePath in filePaths)
        {
            List<Product> products = JsonConverter.DeserializeObject<List<Product>>(File.ReadAllText(filePath));
            allProducts.AddRange(products);
        }
        Console.WriteLine("Filter by category or price?");
        Console.Write("Enter C for category, P for price: ");
        string filterType = Console.ReadLine().ToUpper();

        Predicate<Product> criteria;

        if (filterType == "C")
        {
            Console.WriteLine("Choose category: ");
            string category = Console.ReadLine();
            criteria = c => c.Category == category;
        }
        else if (filterType == "P")
        {
            Console.WriteLine("Enter minimum price:");
            decimal minPrice = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Enter maximum price:");
            decimal maxPrice = Convert.ToDecimal(Console.ReadLine());
            criteria = p => p.Price >= minPrice && p.Price <= maxPrice;
        }
        else
        {
            Console.WriteLine("Invalid filter type.");
            return;
        }

        List<Product> filteredProducts = allProducts.FindAll(criteria);
        Action<Product> display = d => Console.WriteLine($"{d.Name} ({d.Category}) - ${d.Price}");
        filteredProducts.ForEach(display);
        Console.ReadLine();
    }
}