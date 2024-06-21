using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FirebasePlugin.Entities;
using Google.Cloud.Firestore;
using Google.Protobuf.WellKnownTypes;
using Plugin;
using PluginCommon;
using SitePlugin;
using YouTubeLiveSitePlugin;

namespace FirebasePlugin
{
    public class Model : INotifyPropertyChanged
    {
        private readonly IOptions _options;
        private readonly IPluginHost _host;

        public string FirebaseProjectId
        {
            get => _options.FirebaseProjectId;
            set => _options.FirebaseProjectId = value;
        }
        public string FirebaseConfigJsonPath
        {
            get => _options.FirebaseConfigJsonPath;
            set => _options.FirebaseConfigJsonPath = value;
        }
        public string FirestoreYouTubeLiveCommentCollectionPath
        {
            get => _options.FirestoreYouTubeLiveCommentCollectionPath;
            set => _options.FirestoreYouTubeLiveCommentCollectionPath = value;
        }
        public string FirestoreYouTubeUserCollectionPath
        {
            get => _options.FirestoreYouTubeUserCollectionPath;
            set => _options.FirestoreYouTubeUserCollectionPath = value;
        }
        public string FirestoreYouTubeLiveConnectedCollectionPath
        {
            get => _options.FirestoreYouTubeLiveConnectedCollectionPath;
            set => _options.FirestoreYouTubeLiveConnectedCollectionPath = value;
        }
        public string FirestoreYouTubeLiveDisconnectedCollectionPath
        {
            get => _options.FirestoreYouTubeLiveDisconnectedCollectionPath;
            set => _options.FirestoreYouTubeLiveDisconnectedCollectionPath = value;
        }
        public double DateWidth
        {
            get => _options.DateWidth;
            set => _options.DateWidth = value;
        }
        public double IdWidth
        {
            get => _options.IdWidth;
            set => _options.IdWidth = value;
        }
        public double NameWidth
        {
            get => _options.NameWidth;
            set => _options.NameWidth = value;
        }
        public double CalledWidth
        {
            get => _options.CalledWidth;
            set => _options.CalledWidth = value;
        }
        public Model(IOptions options, IPluginHost host)
        {
            _options = options;
            _host = host;
        }
        protected virtual DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
        public void ShowFilePicker()
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Firebase 設定 JSON ファイルを選択してください",
                Filter = "JSON ファイル|*.json"
            };
            var result = fileDialog.ShowDialog();
            if (result == true)
            {
                this.FirebaseConfigJsonPath = fileDialog.FileName;
            }
        }
        public async void AddYouTubeLiveMessage(IYouTubeLiveComment youTubeLiveComment)
        {
            if (string.IsNullOrEmpty(_options.FirebaseProjectId)) { throw new ApplicationException("Firebase プロジェクト ID が未指定です。"); }
            FirestoreDb db = FirestoreDb.Create(_options.FirebaseProjectId);
            if (string.IsNullOrEmpty(_options.FirestoreYouTubeLiveCommentCollectionPath)) { throw new ApplicationException("Firestore YouTube Live Comment Collection パスが未指定です。"); }
            CollectionReference collectionRef = db.Collection(_options.FirestoreYouTubeLiveCommentCollectionPath);
            await collectionRef.AddAsync(
                new YouTubeLiveChatMessage() {
                    YouTubeUserRef = await AddYouTubeUser(youTubeLiveComment), 
                    MessageType = youTubeLiveComment.YouTubeLiveMessageType.ToString(),
                    MessageId = youTubeLiveComment.Id,
                    Text = youTubeLiveComment.CommentItems.ToString(),
                    PostedAt = Google.Cloud.Firestore.Timestamp.FromDateTime(youTubeLiveComment.PostedAt.ToUniversalTime()),
                });
        }
        public async void AddYouTubeLiveMessage(IYouTubeLiveConnected message)
        {
            if (string.IsNullOrEmpty(_options.FirebaseProjectId)) { throw new ApplicationException("Firebase プロジェクト ID が未指定です。"); }
            FirestoreDb db = FirestoreDb.Create(_options.FirebaseProjectId);
            if (string.IsNullOrEmpty(_options.FirestoreYouTubeLiveConnectedCollectionPath)) { throw new ApplicationException("Firestore YouTube Live Connected Collection パスが未指定です。"); }
            CollectionReference collectionRef = db.Collection(_options.FirestoreYouTubeLiveConnectedCollectionPath);
            await collectionRef.AddAsync(
                new
                {
                    text = message.Text,
                    messageType = message.YouTubeLiveMessageType.ToString(),
                });
        }
        public async void AddYouTubeLiveMessage(IYouTubeLiveDisconnected message)
        {
            if (string.IsNullOrEmpty(_options.FirebaseProjectId)) { throw new ApplicationException("Firebase プロジェクト ID が未指定です。"); }
            FirestoreDb db = FirestoreDb.Create(_options.FirebaseProjectId);
            if (string.IsNullOrEmpty(_options.FirestoreYouTubeLiveDisconnectedCollectionPath)) { throw new ApplicationException("Firestore YouTube Live Disconnected Collection パスが未指定です。"); }
            CollectionReference collectionRef = db.Collection(_options.FirestoreYouTubeLiveDisconnectedCollectionPath);
            await collectionRef.AddAsync(
                new 
                {
                    text = message.Text,
                    messageType = message.YouTubeLiveMessageType.ToString(),
                });
        }
        public async Task<DocumentReference> AddYouTubeUser(IYouTubeLiveComment youTubeLiveComment)
        {
            if (string.IsNullOrEmpty(_options.FirebaseProjectId)) { throw new ApplicationException("Firebase プロジェクト ID が未指定です。"); }
            FirestoreDb db = FirestoreDb.Create(_options.FirebaseProjectId);
            if (string.IsNullOrEmpty(_options.FirestoreYouTubeUserCollectionPath)) { throw new ApplicationException("Firestore YouTube User Collection パスが未指定です。"); }
            CollectionReference collectionRef = db.Collection(_options.FirestoreYouTubeUserCollectionPath);
            var snapshot = await collectionRef.Document(youTubeLiveComment.UserId).GetSnapshotAsync();
            DocumentReference docRef;
            if (snapshot.Exists)
            {
                docRef = snapshot.Reference;
            }
            else
            {
                docRef = collectionRef.Document(youTubeLiveComment.UserId);
            }
            await docRef.SetAsync(
              new YouTubeUser()
              {
                  Id = docRef.Id,
                  IconUrl = youTubeLiveComment.UserIcon.Url,
                  DisplayName = youTubeLiveComment.NameItems.ToText(),
                  PostedAt = Google.Cloud.Firestore.Timestamp.FromDateTime(youTubeLiveComment.PostedAt.ToUniversalTime()),
              }, SetOptions.MergeAll);

            return docRef;
        }
        public bool IsEnabled
        {
            get => _options.IsEnabled;
            set => _options.IsEnabled = value;
        }
        private void WriteComment(string comment)
        {
            _host.PostCommentToAll(comment);
        }

        #region INotifyPropertyChanged
        [NonSerialized]
        private System.ComponentModel.PropertyChangedEventHandler _propertyChanged;

        /// <summary>
        /// 
        /// </summary>
        public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            _propertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
