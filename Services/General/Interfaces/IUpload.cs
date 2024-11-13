using video_migration_tool.Models;

namespace video_migration_tool.Services.Interfaces;

public interface IUpload
{
    public Task UploadAsync(IntegrationFileInfo fileInfo);
}