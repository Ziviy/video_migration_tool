using System.ComponentModel.DataAnnotations;

namespace encouraging_bot.TelegramBot;

public class TelegramConfig
{
    [Required]
    public string Token {get; set; } = String.Empty;
}