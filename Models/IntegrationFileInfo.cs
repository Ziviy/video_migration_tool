namespace video_migration_tool.Models;

public class IntegrationFileInfo
{
    
    public IntegrationFileInfo(string fileName, string filePath)
    {
        FileName = fileName;
        FilePath = filePath;
    }

    public readonly string FilePath;
    public readonly string FileName;
    
}