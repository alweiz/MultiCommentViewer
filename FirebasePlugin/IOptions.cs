using System.ComponentModel;

namespace FirebasePlugin
{
    public interface IOptions : INotifyPropertyChanged
    {
        string FirebaseProjectId { get; set; }
        string FirebaseConfigJsonPath { get; set; }
        string FirestoreYouTubeLiveCommentCollectionPath { get; set; }
        string FirestoreYouTubeUserCollectionPath { get; set; }
        string FirestoreYouTubeLiveConnectedCollectionPath { get; set; }
        string FirestoreYouTubeLiveDisconnectedCollectionPath { get; set; }
        bool IsEnabled { get; set; }
        double DateWidth { get; set; }
        double IdWidth { get; set; }
        double NameWidth { get; set; }
        double CalledWidth { get; set; }

        void Deserialize(string s);
        string Serialize();
        void Reset();
    }
}