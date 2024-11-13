using System.ComponentModel.DataAnnotations;

namespace video_migration_tool.TelegramBot;

public class Config
{
    public VK? VK { get; set; }
    [Required]
    public Telegram Telegram { get; set; }
}  
    public class Telegram
    {
        [Required]
        public string Token { get; set; }
    }
    
    public class VK
    {
        public string Token { get; set; } = String.Empty;
        public string ApplicationId { get; set; }
    }
