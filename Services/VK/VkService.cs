using System.Net;
using video_migration_tool.Models;
using video_migration_tool.Services.Interfaces;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace video_migration_tool.Services.VK;

public class VkService : IUpload
{
    private readonly IGeneralServices _generalServices;
    private readonly IVkApi _vkapi;

    public VkService(
        IGeneralServices generalServices,
        IVkApi vkapi)
    {
        _generalServices = generalServices;
        _vkapi = vkapi;
    }

    public async Task UploadAsync(IntegrationFileInfo fileInfo)
    {
        var @params = new VideoSaveParams()
        {
            Name = fileInfo.FilePath,
            IsPrivate = true
        };
        Console.WriteLine("1111111111111111111");
        var save = _vkapi.Video.Save(@params);
        Console.WriteLine("222222222222");
        var webClient = new WebClient();
        var httpClient = _generalServices.CreateHttpClient();
        byte[] responseArray = await webClient.UploadFileTaskAsync(save.UploadUrl, fileInfo.FileName);
    }
}