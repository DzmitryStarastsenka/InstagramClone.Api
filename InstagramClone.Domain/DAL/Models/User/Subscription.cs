namespace InstagramClone.Domain.DAL.Models.User
{
    public class Subscription
    {
        public int SubscriberId { get; set; }
        public UserProfile Subscriber { get; set; }

        public int PublisherId { get; set; }
        public UserProfile Publisher { get; set; }
    }
}