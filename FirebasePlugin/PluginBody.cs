using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Threading;
using Plugin;
using System.ComponentModel.Composition;
using SitePlugin;
using Google.Cloud.Firestore;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;
using BigoSitePlugin;
using LineLiveSitePlugin;
using MildomSitePlugin;
using MirrativSitePlugin;
using MixchSitePlugin;
using NicoSitePlugin;
using OpenrecSitePlugin;
using PeriscopeSitePlugin;
using ShowRoomSitePlugin;
using TwicasSitePlugin;
using TwitchSitePlugin;
using WhowatchSitePlugin;
using YouTubeLiveSitePlugin;
using PluginCommon;

namespace FirebasePlugin
{
    [Export(typeof(IPlugin))]
    public class PluginBody : IPlugin
    {
        private IOptions _options;

        public string Name
        {
            get
            {
                return "Firebaseプラグイン";
            }
        }
        public string Description
        {
            get
            {
                return "";
            }
        }
        public IPluginHost Host { get; set; }

        public void OnMessageReceived(ISiteMessage message, IMessageMetadata messageMetadata)
        {
            if (!_options.IsEnabled || messageMetadata.IsNgUser || messageMetadata.IsInitialComment || messageMetadata.Is184)
                return;

            IYouTubeLiveComment comment = message as IYouTubeLiveComment;
            if (null != comment)
            {
                _model.AddYouTubeUser(comment);
                _model.AddYouTubeLiveMessage(comment);
            }
            IYouTubeLiveConnected connected = message as IYouTubeLiveConnected;
            if (null != connected)
            {
                _model.AddYouTubeLiveMessage(connected);
            }
            IYouTubeLiveDisconnected disconnected = message as IYouTubeLiveDisconnected;
            if (null != disconnected)
            {
                _model.AddYouTubeLiveMessage(disconnected);
            }
        }
        SettingsViewModel _vm;
        private Dispatcher _dispatcher;
        protected virtual IOptions LoadOptions()
        {
            var options = new DynamicOptions();
            try
            {
                var s = Host.LoadOptions(GetSettingsFilePath());
                options.Deserialize(s);
            }
            catch (System.IO.FileNotFoundException) { }
            return options;
        }
        public void OnLoaded()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _options = LoadOptions();
            _model = CreateModel();
            _vm = CreateSettingsViewModel();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _model.FirebaseConfigJsonPath);
        }

        protected virtual SettingsViewModel CreateSettingsViewModel()
        {
            return new SettingsViewModel(_model, _dispatcher);
        }
        protected virtual Model CreateModel()
        {
            return new Model(_options, Host);
        }

        public void OnClosing()
        {
            var s = _options.Serialize();
            Host.SaveOptions(GetSettingsFilePath(), s);
        }
        public void Run()
        {
        }

        public void ShowSettingView()
        {
            var left = Host.MainViewLeft;
            var top = Host.MainViewTop;
            var view = new SettingsView
            {
                Left = left,
                Top = top,
                DataContext = _vm
            };
            view.Show();
        }

        public string GetSettingsFilePath()
        {
            var dir = Host.SettingsDirPath;
            return Path.Combine(dir, $"{Name}.txt");
        }

        public void OnTopmostChanged(bool isTopmost)
        {
            if (_vm != null)
            {
                _vm.Topmost = isTopmost;
            }
        }
        Model _model;
        public PluginBody()
        {
        }
    }
}
