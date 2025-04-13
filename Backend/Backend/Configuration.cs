namespace Backend;

public static class Configuration
{
    /// <summary>
    /// Configures an alternative Binding to a Port if provided in the "WEBAPI_PORT" Environment Variable
    /// </summary>
    /// <param name="app"></param>
    internal static void CustomBinding(WebApplication app)
    {
        var webapiPort = System.Environment.GetEnvironmentVariable("WEBAPI_PORT");
        if (webapiPort is null) return;
        app.Urls.Clear();
        app.Urls.Add($"http://[::]:{webapiPort}");
    }
}