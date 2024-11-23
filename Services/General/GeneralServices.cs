using System.Net;
using video_migration_tool.Services.Interfaces;

namespace video_migration_tool.Services.General;

public class GeneralServices : IGeneralServices
{
    public HttpClient CreateHttpClient()
    {
        var webClient = new HttpClient();
        return webClient;
    }
}