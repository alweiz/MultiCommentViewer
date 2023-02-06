using Common;
using Moq;
using NUnit.Framework;
using FirebasePlugin;
using Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebasePluginTests
{
    [TestFixture]
    class ModelTests
    {
        [Test]
        public void FirebaseTest()
        {
            IOptions options = new DynamicOptions();
            var hostMock = new Mock<IPluginHost>();
            var host = hostMock.Object;
            var user = new UserTest("abc");
            var model = new Model(options, host);
        }
    }
}
