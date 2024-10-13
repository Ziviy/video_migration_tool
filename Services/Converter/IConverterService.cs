using encouraging_bot.Hostings.YouTube;

namespace encouraging_bot.Services.Converter;

public interface IConverterService
{
    public Task ConvertAsync(VideoObj video, CancellationToken cancellationToken = default);
}