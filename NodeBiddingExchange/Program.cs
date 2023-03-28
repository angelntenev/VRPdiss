/**************************************************************************
 *                                                                        *
 *  Description: Simple example of using the ActressMas framework         *
 *  Website:     https://github.com/florinleon/ActressMas                 *
 *  Copyright:   (c) 2018, Florin Leon                                    *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

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