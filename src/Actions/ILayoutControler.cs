namespace Limaki.Actions {
    /// <summary>
    /// Executes a ICommand-Collection
    /// </summary>
    public interface ILayoutControler:IAction {

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
        ILayout Layout { get;set;}

    }

    /// <summary>
    /// Decouples Data (Model) from User-Commands (View)
    /// uses a Layout
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public interface ILayoutControler<TData,TItem>:ILayoutControler {
        TData Data { get; }
        void Execute(ICommand<TItem> command);
        ILayout<TData, TItem> Layout {get;set;}
    } 
}