using System;
using System.Collections.Generic;


namespace Limaki.Data {

    /// <summary>
    /// base interface for domain specific quore
    /// </summary>
    public interface IDomainQuore : IDisposable {
        
        IQuore Quore { get; }

    }
    
}
