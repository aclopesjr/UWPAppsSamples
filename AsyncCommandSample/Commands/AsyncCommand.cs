using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCommandSample.Commands
{
    public class AsyncCommand : AsyncBaseCommand
    {
        private Func<Task> func;

        public AsyncCommand(Func<Task> func)
        {
            this.func = func;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override Task ExecuteAsync(object parameter)
        {
            return func();
        }
    }
}
