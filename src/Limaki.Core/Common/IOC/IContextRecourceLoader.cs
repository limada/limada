namespace Limaki.Common.IOC {
    public interface IContextRecourceLoader {
        /// <summary>
        /// instruments the context
        /// </summary>
        /// <param name="context"></param>
        void ApplyResources(IApplicationContext context);
    }
}