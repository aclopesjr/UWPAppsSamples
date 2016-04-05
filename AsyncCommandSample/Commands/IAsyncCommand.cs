using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncCommandSample.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
