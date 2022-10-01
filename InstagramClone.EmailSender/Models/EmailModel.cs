namespace InstagramClone.EmailSender.Models
{
    public class EmailModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PostEmailModel Post { get; set; }
        public string ShareLink { get; set; }
    }
}
