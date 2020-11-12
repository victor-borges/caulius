using System.Threading.Tasks;

namespace Caulius.Client.Handlers
{
    public interface IMessageHandler
    {
        public Task SetupHandlerAsync();
    }
}
