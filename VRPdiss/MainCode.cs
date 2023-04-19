using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace VRPdiss
{
    class MainCode
    {
        static void Main(string[] args)
        {
            for (int fileIndex = 1; fileIndex <= 300; fileIndex++)
            {
                string inputFilePath = $"C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Split Routes\\SeparatedRoute{fileIndex}.txt";
                string outputFilePath = $"C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Solution to Routes\\Route{fileIndex}Solution.txt";

                // Define the parameters for the Genetic Algorithm
                int populationSize = 100;
                int numberOfGenerations = 10000;
                double mutationRate = 0.7;
                double crossoverRate = 0.05;

                // Initialize the population
                List<Route> population = GenerateInitialPopulation(populationSize, inputFilePath);

                // Evaluate the fitness of each individual in the population
                EvaluateFitness(population);
                Route solution = GetBestIndividual(population);

                // Loop through the number of generations
                for (int i = 0; i < numberOfGenerations; i++)
                {
                    // Select individuals for mating
                    List<Route> matingPool = SelectMatingIndividuals(population, population.Count());

                    // Crossover the individuals to create offspring
                    List<Route> offspring = Crossover(matingPool, crossoverRate);

                    // Mutate the offspring
                    Mutate(offspring, mutationRate);

                    // Evaluate the fitness of the offspring
                    EvaluateFitness(offspring);

                    solution = GetBestIndividual(population);

                    // Select the next generation from the population and offspring
                    population = SelectNextGeneration(population, offspring);

                }

                // The best individual in the final population is the solution to the MDVRP
                solution = GetBestIndividual(population);

                using (StreamWriter sw = new StreamWriter(outputFilePath))
                {
                    for (int i = 0; i < solution.getCustomers().Count; i++)
                    {
                        sw.WriteLine(solution.getCustomers()[i].getLocation().X + "," + solution.getCustomers()[i].getLocation().Y);
                    }
                }

                Console.WriteLine("Solution: " + solution.getTotalDistance());
            }
        }


        // Generate the initial population
        private static List<Route> GenerateInitialPopulation(int populationSize, string inputFilePath)
        {
            List<Route> population = new List<Route>();

            for (int i = 0; i < populationSize; i++)
            {
                Route route = new Route(true, inputFilePath);

                // Shuffle the list of customers
                List<Customer> shuffledCustomers = route.getCustomers().OrderBy(x => Guid.NewGuid()).ToList();

                // Insert the first and last customers at the beginning and end of the list, respectively
                shuffledCustomers.Insert(0, shuffledCustomers.Last());
                shuffledCustomers.RemoveAt(shuffledCustomers.Count - 1);

                // Add the shuffled customers to the route
                foreach (Customer customer in shuffledCustomers)
                {
                    route.addCustomer(customer);
                }

                population.Add(route);
            }

            return population;
        }






        // Evaluate the fitness of each individual
        static void EvaluateFitness(List<Route> population)
        {
            for (int i = 0; i < population.Count; i++)
            {
                population[i].CalculateTotalDistance();
                Console.WriteLine("Route " + i + " distance: " + population[i].getTotalDistance());
            }
        }

        // Select individuals for mating
        private static List<Route> SelectMatingIndividuals(List<Route> population, int matingPoolSize)
        {
            List<Route> matingPool = new List<Route>();

            for (int i = 0; i < matingPoolSize; i++)
            {
                List<Route> tournament = new List<Route>();
                for (int j = 0; j < 5; j++)
                {
                    Random rnd = new Random();
                    int randomIndex = rnd.Next(0, population.Count - 1);
                    tournament.Add(population[randomIndex]);

                }
                tournament = tournament.OrderBy(r => r.getTotalDistance()).ToList();
                matingPool.Add(tournament[0]);
            }

            return matingPool;

        }


        // Crossover the individuals to create offspring
        private static List<Route> Crossover(List<Route> matingPool, double crossoverRate)
        {
            List<Route> offspring = new List<Route>();

            while (offspring.Count < matingPool.Count)
            {
                Random rnd = new Random();
                int parent1Index = rnd.Next(0, matingPool.Count - 1);
                int parent2Index = rnd.Next(0, matingPool.Count - 1);

                while (parent1Index == parent2Index)
                {
                    parent2Index = rnd.Next(0, matingPool.Count - 1);
                }


                Route parent1 = matingPool[parent1Index];
                Route parent2 = matingPool[parent2Index];


                if (rnd.NextDouble() < crossoverRate)
                {

                    Route offspring1 = new Route(false);
                    Route offspring2 = new Route(false);

                    for (int i = 0; i < parent1.getCustomers().Count; i++)
                    {
                        if ((parent1.getCustomer(i).getLocation() != parent2.getCustomer(i).getLocation()))
                        {
                            
                                offspring1.addCustomer(parent1.getCustomer(i));
                                offspring2.addCustomer(parent2.getCustomer(i));
                        }
                        else
                        {
                                offspring1.addCustomer(parent2.getCustomer(i));
                                offspring2.addCustomer(parent1.getCustomer(i));
                        }
                    }

                    offspring1.CalculateTotalDistance();
                    offspring2.CalculateTotalDistance();

                    offspring.Add(offspring1);
                    offspring.Add(offspring2);
                }
            }

            return offspring;
        }


        // Mutate the offspring
        private static List<Route> Mutate(List<Route> population, double mutationRate)
        {
            List<Route> mutatedPopulation = new List<Route>();

            foreach (Route route in population)
            {
                Random rnd = new Random();
                if (rnd.NextDouble() < mutationRate)
                {
                    int customer1Index = rnd.Next(0, route.getCustomers().Count - 1);
                    int customer2Index = rnd.Next(0, route.getCustomers().Count - 1);
                    
                    while (customer1Index == customer2Index)
                    {
                        customer2Index = rnd.Next(0, route.getCustomers().Count - 1);
                    }

                    Customer customer1 = route.getCustomer(customer1Index);
                    Customer customer2 = route.getCustomer(customer2Index);

                    route.setCustomer(customer1Index, customer2);
                    route.setCustomer(customer2Index, customer1);
                }

                mutatedPopulation.Add(route);
            }

            return mutatedPopulation;
        }


        // Select the next generation from the population and offspring
        private static List<Route> SelectNextGeneration(List<Route> currentPopulation, List<Route> offspringPopulation)
        {
            List<Route> nextGeneration = new List<Route>();

            // Sort the offspring population by fitness
            offspringPopulation = offspringPopulation.OrderByDescending(x => x.getTotalDistance()).ToList();

            // Select the top individuals from the offspring population to form the next generation
            for (int i = 0; i < currentPopulation.Count; i++)
            {
                nextGeneration.Add(offspringPopulation[i]);
            }

            return nextGeneration;
        }



        // Get the best individual in the population
        private static Route GetBestIndividual(List<Route> population)
        {
            Route bestIndividual = population[0];

            foreach (Route individual in population)
            {
                
                if (individual.getTotalDistance() < bestIndividual.getTotalDistance())
                {
                    bestIndividual = individual;
                }
            }
            
            return bestIndividual;
        }


        public static Route GenerateRandomRoute()
        {
            Route route = new Route(false);
            List<int> customerIds = Enumerable.Range(0, route.getCustomers().Count).ToList();

            while (customerIds.Count > 0)
            {
                Random rnd = new Random();
                int customerIndex = rnd.Next(0, customerIds.Count - 1);
                int customerId = customerIds[customerIndex];

                route.addCustomer(route.getCustomer(customerId));
                customerIds.RemoveAt(customerIndex);
            }

            route.CalculateTotalDistance();
            return route;
        }

    }
}
