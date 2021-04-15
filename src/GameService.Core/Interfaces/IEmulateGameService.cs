using System.Threading.Tasks;

namespace GameService.Core.Interfaces
{
    public interface IEmulateGameService
    {
        Task<bool> PlayGame(bool isReadyToChange);
    }
}
