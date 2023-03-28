using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPdiss
{
    internal class Route
    {
        // List of customers in the route
        private List<Customer> Customers { get; set; }

        // Total distance traveled by the vehicle in the route
        private double TotalDistance { get; set; }

        // Constructor to initialize the Customers list
        public Route(bool basePopulation, string filePath = null)
        {
            Customers = new List<Customer>();
            if (basePopulation == true)
            {
                // If filePath is not provided, use the default path
                if (filePath == null)
                {
                    filePath = "C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\GeneratedRoute1.txt";
                }

                // Print the file path to check if it's not null
                Console.WriteLine("File path: " + filePath);

                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Equals("NEXT"))
                        {
                            break;
                        }
                        else
                        {
                            string[] coordinates = line.Split(',');
                            int x = int.Parse(coordinates[0]);
                            int y = int.Parse(coordinates[1]);
                            Customers.Add(new Customer(x, y));
                        }
                    }
                }
            }
        }


        private void SaveRouteToFile(int routeIndex, List<Customer> customers)
        {
            string folderPath = "C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Split Routes\\";
            string fileName = $"SeparatedRoute{routeIndex}.txt";
            string fullPath = Path.Combine(folderPath, fileName);

            using (StreamWriter sw = new StreamWriter(fullPath))
            {
                foreach (Customer customer in customers)
                {
                    sw.WriteLine($"{customer.getLocation().X},{customer.getLocation().Y}");
                }
            }
        }

        // Method to calculate the TotalDistance
        public void CalculateTotalDistance()
        {
            // Code to calculate the total distance goes here
            TotalDistance = 0;
            for (int i = 0; i < Customers.Count; i++)
            {
                if (i == 0)
                {
                    TotalDistance += MathFormulas.calculateDistanceBetweenCoords(0, 0, Customers[i].getLocation().X, Customers[i].getLocation().Y);
                }
                else
                {
                    TotalDistance += MathFormulas.calculateDistanceBetweenCoords(Customers[i - 1].getLocation().X, Customers[i - 1].getLocation().Y, Customers[i].getLocation().X, Customers[i].getLocation().Y);
                }
            }
        }


        public void setCustomer(int index, Customer customer) { Customers[index] = customer; }
        public Customer getCustomer(int index) { return Customers[index]; }

        public List<Customer> getCustomers() { return Customers; }

        public void addCustomer(Customer customer) { Customers.Add(customer); }
        public double getTotalDistance() 
        {
            TotalDistance = 0;
            for (int i = 0; i < Customers.Count; i++)
            {
                
                if (i == 0)
                {
                    TotalDistance += MathFormulas.calculateDistanceBetweenCoords(0, 0, Customers[i].getLocation().X, Customers[i].getLocation().Y);
                }
                else
                {
                    TotalDistance += MathFormulas.calculateDistanceBetweenCoords(Customers[i - 1].getLocation().X, Customers[i - 1].getLocation().Y, Customers[i].getLocation().X, Customers[i].getLocation().Y);
                }
            }
            return TotalDistance; 
        }

        


    }
}
