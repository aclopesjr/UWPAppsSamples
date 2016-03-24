using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackgroundDownloadSample.Commands
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action execute;
        private Func<bool> canExecute;

        public Command(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute ?? (() => true);
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute();
        }

        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
                this.execute();
        }
    }
}
