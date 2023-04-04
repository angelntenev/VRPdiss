using ActressMas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRPdiss;

namespace NodeBiddingExchange
{
    class NodeBiddingAuction : Agent
    {
        double distance = 0;
        Point furthestPoint1 = new Point();
        Point furthestPoint2 = new Point();
        Point furthestPoint3 = new Point();

        List<Point> route = new List<Point>();

        List<AgentDetailsForTrade> furthestPointstoConsider = new List<AgentDetailsForTrade>();
        List<string> agentsRequestingHelp = new List<string>();

        List<string> leastDistanceOfferedList = new List<string>();
        double leastDistanceOffered;

        double distanceSaved = 0;
        
        int numberOfAgents = 0;

        bool readyToStop = false;
        bool active = false;

        public override void Setup()
        {
            // Open the text file using a StreamReader
            StreamReader reader = new StreamReader("C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Routes for auction\\Route30.txt");

            // Declare an array of lists of type Point
            List<Point>[] pointsArray = new List<Point>[10];

            // Initialize each element to a new instance of List<Point>
            for (int i = 0; i < pointsArray.Length; i++)
            {
                pointsArray[i] = new List<Point>();
            }

            // Loop through each line of the file
            int arrayIndex = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Trim().ToUpper() == "NEXT" )
                {
                    // Move onto the next element in the array
                    arrayIndex++;
                    continue;
                }

                string[] values = line.Split(',');
                if (values.Length < 2)
                {
                    // Handle invalid input
                    Console.WriteLine(line);
                    continue;
                }
                int x = int.Parse(values[0]);
                int y = int.Parse(values[1]);
                Point point = new Point(x, y);
                pointsArray[arrayIndex].Add(point);
            }


            // Close the StreamReader
            reader.Close();
            char agentChar = Name[^1];

            int toIntAgentNumber = (int)agentChar - 48;

            foreach (Point point in pointsArray[toIntAgentNumber])
            {
                route.Add(point);
            }

            Console.WriteLine("Agent " + Name + " has been created");
            for (int i = 0; i < route.Count; i++)
            {
                Console.WriteLine(route[i]);
            }

            Send("Environment", "start");
        }

        public override void Act(Message message)
        {
            Console.WriteLine($"\t{message.Format()}");
            message.Parse(out string action, out string parameters);


            switch (action)
            {
                case "inform":
                    findFurthestPoint(parameters);
                    break;
                case "broadcast":
                    createListAndOffer(parameters, message);
                    break;
                case "offer":
                    findLowestOffer(parameters, message);
                    break;
                case "acceptOffer":
                    startExchange(parameters, message);
                    break;
                case "pointAccepted":
                    completeExchange(parameters, message);
                    break;
                case "requesterOut":
                    takeOutRequester(parameters, message);
                    break;
                case "die":
                    Console.WriteLine($"{Name} out");
                    active = false;
                    Send("Environment", $"getResults");
                    Stop();
                    break;
                case "restart":
                    Setup();
                    break;
            }
        }

        public override void ActDefault()
        {
            if (readyToStop == true)
            {
                readyToStop = false;
                Send("Environment", "allOut");
            }
        }

        private void findFurthestPoint(string message)
        {
            double tempDistance = 0;
            for (int i = 0; i < route.Count - 3; i++)
            {
                tempDistance = MathFormulas.calculateDistanceBetweenPoints(route[i], route[i + 1]) + MathFormulas.calculateDistanceBetweenPoints(route[i + 2], route[i + 1]);
                if (tempDistance > distance)
                {
                    distance = tempDistance;
                    furthestPoint1 = route[i];
                    furthestPoint2 = route[i+1];
                    furthestPoint3 = route[i+2];

                }
            }

            leastDistanceOffered = distance;
            //Console.WriteLine(distance);
            active = true;
            Broadcast($"broadcast {Name}'s furthest point is {distance} km away at point {furthestPoint2.X},{furthestPoint2.Y}");
        }

        private void createListAndOffer(string parameters, Message message)
        {
            if (active == true)
            {
                string[] parametersArr = parameters.Split(" ");
                numberOfAgents = 10;
                Point furthestPointinList = new Point();

                string[] PointArr = parametersArr[10].Split(",");
                furthestPointinList.X = Int32.Parse(PointArr[0]);
                furthestPointinList.Y = Int32.Parse(PointArr[1]);

                AgentDetailsForTrade tempAgentDetails = new AgentDetailsForTrade(message.Sender[^1] - 48, Double.Parse(parametersArr[5]), message.Sender, furthestPointinList);

                furthestPointstoConsider.Add(tempAgentDetails);
                agentsRequestingHelp.Add(message.Sender);

                if (furthestPointstoConsider.Count == 9)
                {
                    furthestPointstoConsider = furthestPointstoConsider.OrderByDescending(x => x.getAgentDistance()).ToList();

                    double nearestNeigbourDistance = 0;
                    for (int i = 0; i < route.Count - 2; i++)
                    {
                        if (nearestNeigbourDistance < (MathFormulas.calculateDistanceBetweenPoints(route[i], furthestPointstoConsider[0].getFurthestPoint()) + MathFormulas.calculateDistanceBetweenPoints(route[i + 1], furthestPointstoConsider[0].getFurthestPoint())))
                        {
                            nearestNeigbourDistance = MathFormulas.calculateDistanceBetweenPoints(route[i], furthestPointstoConsider[0].getFurthestPoint()) + MathFormulas.calculateDistanceBetweenPoints(route[i + 1], furthestPointstoConsider[0].getFurthestPoint());
                        }
                    }

                    Send(furthestPointstoConsider[0].getAgentName(), $"offer {nearestNeigbourDistance} by {Name}");
                }
            }
        }

        private void findLowestOffer(string parameters, Message message)
        {
            if (active == true)
            {
                string[] parametersArr = parameters.Split(" ");

                leastDistanceOfferedList.Add(parametersArr[0] + "," + message.Sender);

                if (leastDistanceOfferedList.Count == numberOfAgents - 1)
                {
                    string leastOfferAgentName = "";
                    bool validTraderFound = false;
                    foreach (string distance in leastDistanceOfferedList)
                    {
                        string[] tempArr = distance.Split(",");
                        if (leastDistanceOffered > double.Parse(tempArr[0]))
                        {
                            leastDistanceOffered = double.Parse(tempArr[0]);
                            leastOfferAgentName = tempArr[1];
                            validTraderFound = true;
                        }

                    }
                    if (validTraderFound == true)
                    {
                        Send(leastOfferAgentName, $"acceptOffer {leastDistanceOffered} from agent {leastOfferAgentName} for point {furthestPoint2.X},{furthestPoint2.Y}");
                        validTraderFound = false;
                    }
                    else
                    {
                        SendToMany(agentsRequestingHelp, $"requesterOut");
                    }
                }
            }
        }

        private void startExchange(string parameters, Message message)
        {
            if (active == true)
            {
                string[] parametersArr = parameters.Split(" ");

                Point pointToPlace = new Point();

                string[] PointArr = parametersArr[7].Split(",");
                pointToPlace.X = Int32.Parse(PointArr[0]);
                pointToPlace.Y = Int32.Parse(PointArr[1]);

                double minDistance = MathFormulas.calculateDistanceBetweenPoints(route[route.Count - 1], pointToPlace) + MathFormulas.calculateDistanceBetweenPoints(pointToPlace, route[route.Count - 2]);
                int positionToPlace = route.Count - 2;

                for (int i = 0; i < route.Count - 1; i++)
                {
                    if ((MathFormulas.calculateDistanceBetweenPoints(route[i], pointToPlace) + MathFormulas.calculateDistanceBetweenPoints(route[i], pointToPlace)) < minDistance)
                    {
                        minDistance = MathFormulas.calculateDistanceBetweenPoints(route[i], pointToPlace) + MathFormulas.calculateDistanceBetweenPoints(route[i], pointToPlace);
                        positionToPlace = i;
                    }
                }

                double maxDistance1 = 0;
                for (int i = 0; i < route.Count - 1; i++)
                {
                    maxDistance1 = maxDistance1 + MathFormulas.calculateDistanceBetweenPoints(route[i], route[i + 1]);
                }
                double maxDistance2 = 0;

                route.Insert(positionToPlace, pointToPlace);

                for (int i = 0; i < route.Count - 1; i++)
                {
                    maxDistance2 = maxDistance2 + MathFormulas.calculateDistanceBetweenPoints(route[i], route[i + 1]);
                }

                Send("Environment", $"addSeller {maxDistance2 - maxDistance1}");
                distanceSaved = maxDistance1 - maxDistance2;
                Send(message.Sender, $"pointAccepted");
            }
        }

        private void completeExchange(string parameters, Message message)
        {
            if (active == true)
            {
                double maxDistance1 = 0;
                for (int i = 0; i < route.Count - 1; i++)
                {
                    maxDistance1 = maxDistance1 + MathFormulas.calculateDistanceBetweenPoints(route[i], route[i + 1]);
                }

                double maxDistance2 = 0;
                route.Remove(furthestPoint2);

                for (int i = 0; i < route.Count - 1; i++)
                {
                    maxDistance2 = maxDistance2 + MathFormulas.calculateDistanceBetweenPoints(route[i], route[i + 1]);
                }

                Send("Environment", $"addBuyer {maxDistance1 - maxDistance2}");
                distanceSaved = maxDistance1 - maxDistance2;
                SendToMany(agentsRequestingHelp, $"requesterOut");
            }
        }

        private void takeOutRequester(string parameters, Message message)
        {
            if (active == true)
            {
                agentsRequestingHelp.Remove(message.Sender);


                for (int i = 0; i < furthestPointstoConsider.Count; i++)
                {
                    if (furthestPointstoConsider[i].getAgentName().Equals(message.Sender))
                    {
                        furthestPointstoConsider.RemoveAt(i);
                    }
                }

                numberOfAgents--;

                Console.WriteLine(agentsRequestingHelp.Count);
                if (agentsRequestingHelp.Count == 0)
                {

                    readyToStop = true;
                }
                else
                {
                    double nearestNeigbourDistance = 0;
                    for (int i = 0; i < route.Count - 2; i++)
                    {
                        if (nearestNeigbourDistance < (MathFormulas.calculateDistanceBetweenPoints(route[i], furthestPointstoConsider[0].getFurthestPoint()) + MathFormulas.calculateDistanceBetweenPoints(route[i + 1], furthestPointstoConsider[0].getFurthestPoint())))
                        {
                            nearestNeigbourDistance = MathFormulas.calculateDistanceBetweenPoints(route[i], furthestPointstoConsider[0].getFurthestPoint()) + MathFormulas.calculateDistanceBetweenPoints(route[i + 1], furthestPointstoConsider[0].getFurthestPoint());
                        }
                    }
                    Send(furthestPointstoConsider[0].getAgentName(), $"offer {nearestNeigbourDistance} by {Name}");
                }
            }
            
        }
    }

}
