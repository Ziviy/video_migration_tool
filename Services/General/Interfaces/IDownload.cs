using video_migration_tool.Models;

namespace video_migration_tool.Services.General.Interfaces;

public interface IDownload
{
    Task<IntegrationFileInfo> Download(string url);
}