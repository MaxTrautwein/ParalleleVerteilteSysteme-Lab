using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

namespace Backend;

public static class Configuration
{
    /// <summary>
    /// Configures an alternative Binding to a Port if provided in the "WEBAPI_PORT" Environment Variable
    /// </summary>
    /// <param name="app"></param>
    internal static void CustomBinding(WebApplication app)
    {
        var webapiPort = Environment.GetEnvironmentVariable("WEBAPI_PORT");
        if (webapiPort is null) return;
        app.Urls.Clear();
        app.Urls.Add($"http://[::]:{webapiPort}");
    }

    /// <summary>
    /// Get a Configuration Value or the Default
    /// Checks in Order:
    /// 1. "<paramref name="key"/>_FILE" Environment Variable for Path to Secret. Returns if Exists
    /// 2: A Environment Variable Directly
    /// 3: the Default
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    internal static string GetSecretOrEnvVar(string key, string defaultValue = "")
    {
        // Check if we have a Secret File Specified
        var secretFile = Environment.GetEnvironmentVariable($"{key}_FILE");
        if (secretFile is not null && File.Exists(secretFile))
        {
           return File.ReadAllText(secretFile);
        }
        return Environment.GetEnvironmentVariable(key) ?? defaultValue;
    }
    
    /// <summary>
    /// Get a Configuration Value or the Default
    /// Checks in Order:
    /// 1. "<paramref name="key"/>_FILE" Environment Variable for Path to Secret. Returns if Exists
    /// 2: A Environment Variable Directly
    /// 3: the Default
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    internal static bool GetSecretOrEnvVar(string key, bool defaultValue = false)
    {
        bool result;
        // Check if we have a Secret File Specified
        var secretFile = Environment.GetEnvironmentVariable($"{key}_FILE");
        if (secretFile is not null && File.Exists(secretFile))
        {
            return bool.TryParse(File.ReadAllText(secretFile), out result) ? result : defaultValue;
        }
        return bool.TryParse(Environment.GetEnvironmentVariable(key), out result) ? result : defaultValue;
    }
}