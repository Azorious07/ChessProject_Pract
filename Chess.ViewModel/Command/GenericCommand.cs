namespace Chess.ViewModel.Command
{
    using System;
    using System.Windows.Input;

    public class GenericCommand : ICommand
    {
        private readonly Func<bool> canExecute;

        private readonly Action action;

        public GenericCommand(Func<bool> canExecute, Action action)
        {
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object _)
        {
            return this.canExecute();
        }

        public void Execute(object _)
        {
            this.action();
        }

        public void FireCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}