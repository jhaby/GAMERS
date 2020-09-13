using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GAMERS_TECH
{
    public class RespondCommand : ICommand
    {

        Action<object> executeMethod;

        public RespondCommand(Action<object> executeMethod)
        {
            this.executeMethod = executeMethod;
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
