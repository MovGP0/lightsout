using System;
using System.Windows.Input;

namespace LightsOut
{
    public sealed class PressSwitchCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }
        
        public void Execute(object parameter)
        {
            if(parameter is ISwitchViewModel @switch)
                Execute(@switch);
        }

        public void Execute(ISwitchViewModel parameter)
        {
            if(parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            
            switch (parameter.State)
            {
                case SwitchState.On:
                    parameter.State = SwitchState.OnPressed;
                    break;

                case SwitchState.Off:
                    parameter.State = SwitchState.OffPressed;
                    break;
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}