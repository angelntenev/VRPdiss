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
                if (dataList.Count == 10)
                {
                    SendToMany(dataList, content);
                }
                break;
            case "addSeller":
                string[] parametersArr = parameters.Split(" ");

                string filePath1 = $"C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Results\\Initial Days\\{message.Sender}.txt";

                // Create the file and write text to it if it doesn't exist
                if (!File.Exists(filePath1))
                {
                    // Create a new file and write the text
                    using (StreamWriter sw = File.CreateText(filePath1))
                    {
                        sw.WriteLine($"{message.Sender} has gained {Convert.ToDouble(parametersArr[0])} km");
                    }
                }
                else
                {
                    // Append the text to the existing file
                    using (StreamWriter sw = File.AppendText(filePath1))
                    {
                            sw.WriteLine($"{message.Sender} has gained {Convert.ToDouble(parametersArr[0])} km");
                    }
                }


                distanceSaved = distanceSaved + Convert.ToDouble(parametersArr[0]);
                break;
            case "addBuyer":
                string[] parametersArr2 = parameters.Split(" ");


                string filePath2 = $"C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Results\\Initial Days\\{message.Sender}.txt";

                // Create the file and write text to it if it doesn't exist
                if (!File.Exists(filePath2))
                {
                    // Create a new file and write the text
                    using (StreamWriter sw = File.CreateText(filePath2))
                    {
                        sw.WriteLine($"{message.Sender} has saved {Convert.ToDouble(parametersArr2[0])} km");
                    }
                }
                else
                {
                    // Append the text to the existing file
                    using (StreamWriter sw = File.AppendText(filePath2))
                    {
                        sw.WriteLine($"{message.Sender} has saved {Convert.ToDouble(parametersArr2[0])} km");
                    }
                }




                distanceExtended = distanceExtended + Convert.ToDouble(parametersArr2[0]);
                Console.WriteLine(distanceExtended);
                break;
            case "allOut":
                    SendToMany(dataList, "die");
                break;
            case "getResults":
                Console.WriteLine($"Distance reduced for agents is {distanceExtended} and distance increased for agents is {distanceSaved} and a social total of {distanceExtended - distanceSaved} km is saved");
                numOfAgents++;
                if (numOfAgents == 10)
                {
                    string inputDirectory = "C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Results\\Initial Days";
                    string outputFilePath = "C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Results\\Initial Days\\CombinedAgents.txt";

                    // Create the output file or clear its contents if it already exists
                    using (FileStream fs = new FileStream(outputFilePath, FileMode.Create))
                    {
                    }

                    // Loop through the agent files and copy their content to the output file
                    for (int i = 1; i <= 10; i++)
                    {
                        string inputFilePath = $"{inputDirectory}\\Agent {i}.txt";

                        if (File.Exists(inputFilePath))
                        {
                            string fileContent = File.ReadAllText(inputFilePath);

                            // Append the content of the current agent file to the output file
                            using (StreamWriter sw = File.AppendText(outputFilePath))
                            {
                                sw.WriteLine(fileContent);
                            }

                            // Delete the Agent file after it has been read
                            File.Delete(inputFilePath);
                        }
                    }
                }

                break;
            default:
                break;
        }
    }
}