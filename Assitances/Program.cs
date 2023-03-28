for (int fileIndex = 1; fileIndex <= 300; fileIndex++)
{
    string solutionFilePath = $"C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Solution to Routes\\Route{fileIndex}Solution.txt";

    // Step 1: Read the content of the solution file
    List<string> lines = File.ReadAllLines(solutionFilePath).ToList();

    // Step 2: Iterate through the list and store non-consecutive repeating lines in a new list
    List<string> updatedLines = new List<string>();
    string previousLine = null;
    foreach (string line in lines)
    {
        if (line != previousLine)
        {
            updatedLines.Add(line);
        }
        previousLine = line;
    }

    // Step 3: Write the updated content back to the solution file
    using (StreamWriter sw = new StreamWriter(solutionFilePath))
    {
        foreach (string line in updatedLines)
        {
            sw.WriteLine(line);
        }
    }
}