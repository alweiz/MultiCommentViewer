using Google.Cloud.Firestore;

namespace FirebasePlugin.Entities
{
    [FirestoreData]
    public class YouTubeUser
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("iconUrl")]
        public string IconUrl { get; set; }

        [FirestoreProperty("displayName")]
        public string DisplayName { get; set; }

        [FirestoreProperty("postedAt")]
        public Timestamp PostedAt { get; set; }

        [FirestoreDocumentCreateTimestamp]
        public Timestamp CreatedAt { get; set; }

        [FirestoreDocumentUpdateTimestamp]
        public Timestamp UpdatedAt { get; set; }

        [FirestoreDocumentReadTimestamp]
        public Timestamp ReadAt { get; set; }
    }
}
