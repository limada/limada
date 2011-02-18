using NUnit.Framework;
using Limaki.Common;

namespace Limaki.Tests.Common
{
    public class FactoryTest : DomainTest {
        [Test]
        public void Test() {
            var factory = new FactoryBase();
            factory.Add<IPool>(() => new Pool());
            
            
            var pool = factory.Create(typeof(IPool));
            Assert.NotNull(pool);
        }

    }
}