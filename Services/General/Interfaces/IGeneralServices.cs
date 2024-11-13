using System.Net;

namespace video_migration_tool.Services.Interfaces;

public interface IGeneralServices
{
    public HttpClient CreateHttpClient();
}