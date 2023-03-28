/*
 * Author: Simon Powers
 * An Environment Agent that sends information to a Household Agent
 * about that household's demand, generation, and prices to buy and sell
 * from the utility company, on that day. Responds whenever pinged
 * by a Household Agent with a "start" message.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ActressMas;

class EnvironmentAgent : Agent
{
    List<string> dataList = new List<string>();


    double distanceSaved = 0;
    double distanceExtended = 0;

    int numOfAgents = 0;

    int newIndexToStart = 0;

  



    public override void Act(Message message)
    {
        Console.WriteLine($"\t{message.Format()}");
        message.Parse(out string action, out string parameters);

        switch (action)
        {
            case "start":
                string senderID = message.Sender; //get the sender's name so we can reply to them
                string content = $"inform";
                dataList.Add(message.Sender.ToString());
                if (dataList.Count == 300)
                {
                    dataList = dataList.OrderBy(s => int.Parse(s.Split(' ')[1])).ToList();
                    for (int i = 0; i < 10; i++)
                    {
                        Send(dataList[i], content); //send the message with this information back to the household agent that requested it
                        newIndexToStart = i;
                    }
                }
                break;
            case "addSeller":
                string[] parametersArr = parameters.Split(" ");
                distanceSaved = distanceSaved + Convert.ToDouble(parametersArr[0]);
                break;
            case "addBuyer":
                string[] parametersArr2 = parameters.Split(" ");
                distanceExtended = distanceExtended + Convert.ToDouble(parametersArr2[0]);
                Console.WriteLine(distanceExtended);
                break;
            case "allOut":
                int tempIndexToStart = newIndexToStart - 9;
                int tempIndexToStop = newIndexToStart + 1;
                for (int i = tempIndexToStart; i < tempIndexToStop; i++)
                {
                    Send(dataList[i], "die");
                }
                break;
            case "getResults":
                Console.WriteLine($"Distance reduced for agents is {distanceExtended} and distance increased for agents is {distanceSaved} and a social total of {distanceExtended - distanceSaved} km is saved");
                numOfAgents++;
                if (numOfAgents == 10)
                {
                    numOfAgents = 0;
                    int newtempIndexToStart = newIndexToStart + 1;
                    int newIndexToStop = newIndexToStart + 10+1;
                    for (int i = newIndexToStart; i < newIndexToStop; i++)
                    {
                        Send(dataList[i], $"inform");
                        newIndexToStart = i;
                    }
                    distanceSaved = 0;
                    distanceExtended = 0;
                    
                }
                break;
            default:
                break;
        }
    }
}