using System;
using System.Windows.Input;

namespace JsonConverterSample.Commands
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
