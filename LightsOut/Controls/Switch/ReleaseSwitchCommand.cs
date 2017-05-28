using System;
using System.Windows.Input;

namespace LightsOut
{
    public sealed class ReleaseSwitchCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ISwitchViewModel @switch)
                Execute(@switch);
        }

        public void Execute(ISwitchViewModel parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            switch (parameter.State)
            {
                case SwitchState.OnPressed:
                    parameter.State = SwitchState.Off;
                    break;

                case SwitchState.OffPressed:
                    parameter.State = SwitchState.On;
                    break;
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}