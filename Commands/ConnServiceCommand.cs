using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GAMERS_TECH
{
    public class RespondCommand : ICommand
    {

        Action<object> executeMethod;
        Func<object, bool> canExecuteMethod;

        public RespondCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            executeMethod(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
