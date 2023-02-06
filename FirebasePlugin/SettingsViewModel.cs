using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;

namespace FirebasePlugin
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Commands
        #endregion
        public bool Topmost
        {
            get
            {
                return _topmost;
            }
            set
            {
                if (_topmost == value) return;
                _topmost = value;
                RaisePropertyChanged();
            }
        }
        public bool IsEnabled
        {
            get { return _model.IsEnabled; }
            set { _model.IsEnabled = value; }
        }
        public string Title
        {
            get { return "Firebase連携用プラグイン"; }
        }
        private bool _topmost;
        public string FirebaseProjectId
        {
            get { return _model.FirebaseProjectId; }
            set { _model.FirebaseProjectId = value; }
        }
        public string FirebaseConfigJsonPath
        {
            get { return _model.FirebaseConfigJsonPath; }
            set { _model.FirebaseConfigJsonPath = value; }
        }
        private RelayCommand _showFilePickerCommand;
        public ICommand ShowFilePickerCommand
        {
            get
            {
                if (_showFilePickerCommand == null)
                {
                    _showFilePickerCommand = new RelayCommand(() =>
                    {
                        _model.ShowFilePicker();
                    });
                }
                return _showFilePickerCommand;
            }
        }
        public string FirestoreYouTubeLiveCommentCollectionPath
        {
            get { return _model.FirestoreYouTubeLiveCommentCollectionPath; }
            set { _model.FirestoreYouTubeLiveCommentCollectionPath = value; }
        }
        public string FirestoreYouTubeUserCollectionPath
        {
            get { return _model.FirestoreYouTubeUserCollectionPath; }
            set { _model.FirestoreYouTubeUserCollectionPath = value; }
        }
        public string FirestoreYouTubeLiveConnectedCollectionPath
        {
            get { return _model.FirestoreYouTubeLiveConnectedCollectionPath; }
            set { _model.FirestoreYouTubeLiveConnectedCollectionPath = value; }
        }
        public string FirestoreYouTubeLiveDisconnectedCollectionPath
        {
            get { return _model.FirestoreYouTubeLiveDisconnectedCollectionPath; }
            set { _model.FirestoreYouTubeLiveDisconnectedCollectionPath = value; }
        }
        public double DateWidth
        {
            get => _model.DateWidth;
            set => _model.DateWidth = value;
        }
        public double IdWidth
        {
            get => _model.IdWidth;
            set => _model.IdWidth = value;
        }
        public double NameWidth
        {
            get => _model.NameWidth;
            set => _model.NameWidth = value;
        }
        public double CalledWidth
        {
            get => _model.CalledWidth;
            set => _model.CalledWidth = value;
        }
        protected virtual DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
        private readonly Model _model;
        private readonly Dispatcher _dispatcher;
        public SettingsViewModel()
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                var options = new DynamicOptions();
                _model = new Model(options, null);
                IsEnabled = true;
            }
        }
        [GalaSoft.MvvmLight.Ioc.PreferredConstructor]
        internal SettingsViewModel(Model model, Dispatcher dispatcher)
        {
            _model = model;
            _dispatcher = dispatcher;
        }
    }
}
