using System;
using Common;

namespace FirebasePlugin
{
    public class DynamicOptions : DynamicOptionsBase, IOptions
    {
        public string FirebaseProjectId { get => GetValue(); set => SetValue(value); }
        public string FirebaseConfigJsonPath { get => GetValue(); set => SetValue(value); }
        public string FirestoreYouTubeLiveCommentCollectionPath { get => GetValue(); set => SetValue(value); }
        public string FirestoreYouTubeUserCollectionPath { get => GetValue(); set => SetValue(value); }
        public string FirestoreYouTubeLiveConnectedCollectionPath { get => GetValue(); set => SetValue(value); }
        public string FirestoreYouTubeLiveDisconnectedCollectionPath { get => GetValue(); set => SetValue(value); }
        public bool IsEnabled { get => GetValue(); set => SetValue(value); }
        public double DateWidth { get => GetValue(); set => SetValue(value); }
        public double IdWidth { get => GetValue(); set => SetValue(value); }
        public double NameWidth { get => GetValue(); set => SetValue(value); }
        public double CalledWidth { get => GetValue(); set => SetValue(value); }
        protected override void Init()
        {
            Dict.Add(nameof(IsEnabled), new Item { DefaultValue = false, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(FirebaseProjectId), new Item { DefaultValue = _FirebaseProjectId, Predicate = s => !string.IsNullOrEmpty(s), Serializer = s => s, Deserializer = s => s });
            Dict.Add(nameof(FirebaseConfigJsonPath), new Item { DefaultValue = _FirebaseConfigJsonPath, Predicate = s => !string.IsNullOrEmpty(s), Serializer = s => s, Deserializer = s => s });
            Dict.Add(nameof(FirestoreYouTubeLiveCommentCollectionPath), new Item { DefaultValue = _FirestoreYouTubeLiveCommentCollectionPath, Predicate = s => !string.IsNullOrEmpty(s), Serializer = s => s, Deserializer = s => s });
            Dict.Add(nameof(FirestoreYouTubeUserCollectionPath), new Item { DefaultValue = _FirestoreYouTubeUserCollectionPath, Predicate = s => !string.IsNullOrEmpty(s), Serializer = s => s, Deserializer = s => s });
            Dict.Add(nameof(FirestoreYouTubeLiveConnectedCollectionPath), new Item { DefaultValue = _FirestoreYouTubeLiveConnectedCollectionPath, Predicate = s => !string.IsNullOrEmpty(s), Serializer = s => s, Deserializer = s => s });
            Dict.Add(nameof(FirestoreYouTubeLiveDisconnectedCollectionPath), new Item { DefaultValue = _FirestoreYouTubeLiveDisconnectedCollectionPath, Predicate = s => !string.IsNullOrEmpty(s), Serializer = s => s, Deserializer = s => s });

            Dict.Add(nameof(DateWidth), new Item { DefaultValue = 106, Predicate = n => n > 0, Serializer = n => n.ToString(), Deserializer = s => double.Parse(s) });
            Dict.Add(nameof(IdWidth), new Item { DefaultValue = 51, Predicate = n => n > 0, Serializer = n => n.ToString(), Deserializer = s => double.Parse(s) });
            Dict.Add(nameof(NameWidth), new Item { DefaultValue = 95, Predicate = n => n > 0, Serializer = n => n.ToString(), Deserializer = s => double.Parse(s) });
            Dict.Add(nameof(CalledWidth), new Item { DefaultValue = 74, Predicate = n => n > 0, Serializer = n => n.ToString(), Deserializer = s => double.Parse(s) });
        }

        private static readonly string _FirebaseProjectId = "Your project ID";
        private static readonly string _FirebaseConfigJsonPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MultiCommentViewer\FirebasePlugin\your-project-id-XXXXXXXXXXXX.json");
        private static readonly string _FirestoreYouTubeLiveCommentCollectionPath = "youTubeLiveChatMessages";
        private static readonly string _FirestoreYouTubeUserCollectionPath = "youTubeUsers";
        private static readonly string _FirestoreYouTubeLiveConnectedCollectionPath = "youTubeLiveConnectionLogs";
        private static readonly string _FirestoreYouTubeLiveDisconnectedCollectionPath = "youTubeLiveConnectionLogs";
    }
}
