namespace UlitMoment;

public static class EnvLoader
{
    /// <summary>
    /// Loads env variables from specified file
    /// </summary>
    /// <param name="filePath"></param>
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var idx = line.IndexOf('=');

            if (idx == -1)
            {
                continue;
            }

            Environment.SetEnvironmentVariable(line[0..idx], line[(idx + 1)..]);
        }
    }
}
