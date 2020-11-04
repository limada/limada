using System;
using Limada.Data;
using Limaki.Common;
using Limaki.Data;
using Limaki.Repository;

namespace Limaki.LinqData.Tests {

    public class Linq2DbThingQuoreFactory : ThingQuoreFactory {
        public override bool Supports (IDbProvider provider) { throw new NotImplementedException (); }
        public override IDbGateway CreateGateway (IDbProvider provider) { throw new NotImplementedException (); }
        public override IQuore CreateQuore (IDbGateway gateway) { throw new NotImplementedException (); }
        public override IThingQuore CreateDomainQuore (Iori iori) { throw new NotImplementedException (); }
        public override IThingQuore CreateThingQuore (Func<IQuore> createQuore) { throw new NotImplementedException (); }
    }

}