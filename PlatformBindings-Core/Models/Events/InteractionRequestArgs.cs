using PlatformBindings.Enums;

namespace PlatformBindings.Models.Events
{
    public class InteractionRequestArgs
    {
        public InteractionType RequestType { get; set; }
        public bool Handled { get; set; } = false;
    }
}