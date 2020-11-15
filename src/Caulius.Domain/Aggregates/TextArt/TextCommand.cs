using Caulius.Domain.Common;

namespace Caulius.Domain.Aggregates.TextArt
{
    public class TextCommand : Entity
    {
        public string Command { get; set; } = null!;
        public string Text { get; set; } = null!;
    }
}
