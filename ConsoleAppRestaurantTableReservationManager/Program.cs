using System;
using System.Collections.Generic;
using System.IO; // містить класи та функції, які дозволяють взаємодіяти з різними операціями введення/виведення (I/O), зокрема роботою з файлами та потоками даних.
using System.Linq; // дозволяє виконувати різноманітні запити до даних, такі як фільтрація, сортування, групування та проекція.

// Main Application Class
public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManager resManager = new ReservationManager();
        resManager.AddRestaurant("A", 10);
        resManager.AddRestaurant("B", 5);

        Console.WriteLine(resManager.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(resManager.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

// Reservation Manager Class
public class ReservationManager
{
    //
    private List<Restaurant> restaurants;

    public ReservationManager()
    {
        restaurants = new List<Restaurant>();
    }

    // Add Restaurant Method
    public void AddRestaurant(string name, int tableCount)
    {
        try
        {
            Restaurant restaurant = new Restaurant();
            restaurant.Name = name;
            restaurant.Tables = Enumerable.Range(1, tableCount).Select(i => new RestaurantTable()).ToList(); // для кожного числа від 1 до tableCount створюється новий об'єкт RestaurantTable.
            restaurants.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // ex.Message дозволяє виводити більш детальну інформацію про саму помилку
        }
    }

    // Load Restaurants From File
    // File
    private void LoadRestaurantsFromFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurant(parts[0], tableCount);
                }
                else
                {
                    
                    Console.WriteLine($"Invalid line: {line}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Find All Free Tables
    public List<string> FindAllFreeTables(DateTime date)
    {
        try
        { 
            List<string> freeTables = new List<string>();
            foreach (var restaurant in restaurants)
            {
                for (int i = 0; i < restaurant.Tables.Length; i++)
                {
                    if (!restaurant.Tables[i].IsBooked(date))
                    {
                        freeTables.Add($"{restaurant.Name} - Table {i + 1}");
                    }
                }
            }
            return freeTables;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<string>();
        }
    }

    public bool BookTable(string restaurantName, DateTime date, int tableNumber)
    {
        foreach (var restaurant in restaurants)
        {
            if (restaurant.Name == restaurantName)
            {
                if (tableNumber < 0 || tableNumber >= restaurant.Tabeles.Length)
                {
                    throw new Exception(null); //Invalid table number
                }

                return restaurant.Tabeles[tableNumber].Book(date);
            }
        }

        throw new Exception(null); //Restaurant not found
    }

     public void SortRestaurantsByAvailabilityForUsers(DateTime date)
    {
        try
        {
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < restaurants.Count - 1; i++)
                {
                    int currentAvailableTables = CountAvailableTablesForRestaurant(restaurants[i], date); // available tables current
                    int nextAvailableTables = CountAvailableTablesForRestaurant(restaurants[i + 1], date); // available tables next

                    if (currentAvailableTables < nextAvailableTables)
                    {
                        // Swap restaurants
                        var temp = restaurants[i];
                        restaurants[i] = restaurants[i + 1];
                        restaurants[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Count available tables in a restaurant
    public int CountAvailableTables(Restaurant restaurant, DateTime date)
     {
        try
        {
            int count = 0;
            foreach (var table in restaurant.Tables)
            {
                if (!table.IsBooked(date))
                {
                    count++;
                }
            }
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
    }
}

// Restaurant Class
public class Restaurant
{
    public string Name; // name
    public public RestaurantTable[] Tabeles; // tables
}

// Table Class
public class RestaurantTable
{
    private List<DateTime> bookedDates; //booked dates

    public RestaurantTable()
    {
        bookedDates = new List<DateTime>();
    }

    // Book
    public bool Book(DateTime date)
    {
        try
        { 
            if (bookedDates.Contains(date))
            {
                return false;
            }
            // Add to bookedDates
            bookedDates.Add(date);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    // Is Booked
    public bool IsBooked(DateTime date)
    {
        return bookedDates.Contains(date);
    }
}
