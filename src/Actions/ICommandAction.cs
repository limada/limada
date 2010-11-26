namespace Limaki.Actions {
    /// <summary>
    /// Executes a ICommand-Collection
    /// </summary>
    public interface ICommandAction:IAction {

        /// <summary>
        /// Prepares the data
        /// </summary>
        void Invoke();
        /// <summary>
        /// Executes ICommands in an ICommand-Collection
        /// </summary>
        void Execute();

        /// <summary>
        /// Revoves all executed commands
        /// </summary>
        void Done();
    }
    public interface ICommandAction<TData,TItem>:ICommandAction {
        TData Data { get; }
        void Execute(ICommand<TItem> command);
    } 
}