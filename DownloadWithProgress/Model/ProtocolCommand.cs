using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DownloadWithProgress.Model
{
    public class ProtocolCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action executionAction;
        private Func<bool> canExecute;

        public ProtocolCommand(Action executionAction, Func<bool> canExecute = null)
        {
            this.executionAction = executionAction;
            this.canExecute = canExecute ?? (() => true);
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute();
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                executionAction();
        }
    }
}
