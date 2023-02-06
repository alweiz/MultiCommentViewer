using Common;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using FirebasePlugin;
using Plugin;
using SitePlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using YouTubeLiveSitePlugin;

namespace FirebasePluginTests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }
        private IYouTubeLiveComment CreateMessage(string name, string message, string userId)
        {
            var messageMock = new Mock<IYouTubeLiveComment>();
            messageMock.Setup(m => m.NameItems).Returns(new List<IMessagePart> { Common.MessagePartFactory.CreateMessageText(name) });
            messageMock.Setup(m => m.CommentItems).Returns(new List<IMessagePart> { Common.MessagePartFactory.CreateMessageText(message) });
            messageMock.Setup(m => m.UserId).Returns(userId);
            return messageMock.Object;
        }
        private IPluginHost CreatePluginHost(IOptions options)
        {
            var hostMock = new Mock<IPluginHost>();
            hostMock.Setup(h => h.LoadOptions(It.IsAny<string>())).Returns((Func<string, string>)(s =>
            {
                return options.Serialize();
            }));
            hostMock.Setup(h => h.SettingsDirPath).Returns("");
            var host = hostMock.Object;
            return host;
        }
        private static SettingsViewModel CreateViewModel(Model model)
        {
            var vmMock = new Mock<SettingsViewModel>(model, Dispatcher.CurrentDispatcher);
            var vm = vmMock.Object;
            return vm;
        }
        private static Model CreateModel(DynamicOptions options, IPluginHost host)
        {
            var modelMock = new Mock<Model>(options, host) { CallBase = true };
            modelMock.Protected().Setup<DateTime>("GetCurrentDateTime").Returns(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime());
            var model = modelMock.Object;
            return model;
        }
        private PluginBody CreatePlugin(SettingsViewModel vm, Model model, IOptions options)
        {
            var pluginMock = new Mock<PluginBody>() { CallBase = true };
            pluginMock.Protected().Setup<SettingsViewModel>("CreateSettingsViewModel").Returns(vm);
            pluginMock.Protected().Setup<Model>("CreateModel").Returns(model);
            pluginMock.Protected().Setup<IOptions>("LoadOptions").Returns(options);
            return pluginMock.Object;
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
    [TestFixture]
    public class コマンドテスト
    {
        PluginBody _plugin;
        SettingsViewModel _vm;
        private IYouTubeLiveComment CreateMessage(string name, string message, string userId)
        {
            var messageMock = new Mock<IYouTubeLiveComment>();
            messageMock.Setup(m => m.NameItems).Returns(new List<IMessagePart> { Common.MessagePartFactory.CreateMessageText(name) });
            messageMock.Setup(m => m.CommentItems).Returns(new List<IMessagePart> { Common.MessagePartFactory.CreateMessageText(message) });
            messageMock.Setup(m => m.UserId).Returns(userId);
            return messageMock.Object;
        }
        private IPluginHost CreatePluginHost(IOptions options)
        {
            var hostMock = new Mock<IPluginHost>();
            hostMock.Setup(h => h.LoadOptions(It.IsAny<string>())).Returns((Func<string, string>)(s =>
            {
                return options.Serialize();
            }));
            hostMock.Setup(h => h.SettingsDirPath).Returns("");
            var host = hostMock.Object;
            return host;
        }
        private static SettingsViewModel CreateViewModel(Model model)
        {
            var vmMock = new Mock<SettingsViewModel>(model, Dispatcher.CurrentDispatcher);
            var vm = vmMock.Object;
            return vm;
        }
        private static Model CreateModel(DynamicOptions options, IPluginHost host)
        {
            var modelMock = new Mock<Model>(options, host) { CallBase = true };
            modelMock.Protected().Setup<DateTime>("GetCurrentDateTime").Returns(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime());
            var model = modelMock.Object;
            return model;
        }
        private PluginBody CreatePlugin(SettingsViewModel vm, Model model, IOptions options)
        {
            var pluginMock = new Mock<PluginBody>() { CallBase = true };
            pluginMock.Protected().Setup<SettingsViewModel>("CreateSettingsViewModel").Returns(vm);
            pluginMock.Protected().Setup<Model>("CreateModel").Returns(model);
            pluginMock.Protected().Setup<IOptions>("LoadOptions").Returns(options);
            return pluginMock.Object;
        }
        [SetUp]
        public void Setup()
        {
            var options = new DynamicOptions()
            {
                IsEnabled = true,
            };
            var host = CreatePluginHost(options);

            var model = CreateModel(options, host);

            var vm = CreateViewModel(model);
            _vm = vm;

            var plugin = CreatePlugin(vm, model, options);
            _plugin = plugin;
            plugin.Host = host;
            plugin.OnLoaded();
        }
        private void AddComment(string comment, string userId)
        {
            var messageMock = new Mock<IYouTubeLiveComment>();
            messageMock.Setup(m => m.NameItems).Returns(new List<IMessagePart> { Common.MessagePartFactory.CreateMessageText("name") });
            messageMock.Setup(m => m.CommentItems).Returns(new List<IMessagePart> { Common.MessagePartFactory.CreateMessageText(comment) });
            messageMock.Setup(m => m.UserId).Returns(userId);
            var message = messageMock.Object;

            var messageMetadataMock = new Mock<IMessageMetadata>();
            messageMetadataMock.Setup(m => m.User).Returns(new UserTest(userId));
            messageMetadataMock.Setup(x => x.SiteContextGuid).Returns(Guid.NewGuid());
            var messageMetadata = messageMetadataMock.Object;

            _plugin.OnMessageReceived(message, messageMetadata);
        }
        [TearDown]
        public void TearDown()
        {
        }
    }
}
