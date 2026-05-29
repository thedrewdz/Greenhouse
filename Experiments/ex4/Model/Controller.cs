namespace Experiment4.Model
{
    public class Controller
    {
        public string ControllerId { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
    }
}
