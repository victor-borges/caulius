using JetBrains.Annotations;

namespace Caulius.Client.Options
{
    [UsedImplicitly]
    public class CauliusOptions
    {
        public string Token { get; set; } = string.Empty;
        public string Prefix { get; set; } = "!";
    }
}
