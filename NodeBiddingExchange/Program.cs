using ActressMas;
using NodeBiddingExchange;
using System;
using System.Threading;
using System.Xml.Linq;

namespace BiddingApp
{
    public class Program
    {

        private static void Main(string[] args)
        {

            var env = new EnvironmentMas();

            //Prompt number choice of agents bidding
            Console.WriteLine("Please enter amount of household agents trading:");
            int numbOfBidders = Int32.Parse(Console.ReadLine());

            //Initialize agents according to number selected
            for (int i = 1; i <= numbOfBidders; i++)
            {
                var traderAgent = new NodeBiddingAuction();
                env.Add(traderAgent, $"Agent {i}");
            }

            //Initialize and start Environment
            var EnvironmentAgent = new EnvironmentAgent();
            env.Add(EnvironmentAgent, "Environment");
            env.Start();

        }

    }



}