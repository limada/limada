namespace Limaki.Actions {
    /// <summary>
    /// Executes a ICommand-Collection
    /// </summary>
    public interface IDataControler:IAction {

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

    /// <summary>
    /// Decouples Data (Model) from User-Commands (View)
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public interface IDataControler<TData,TItem>:IDataControler {
        TData Data { get; }
        void Execute(ICommand<TItem> command);
    } 
}