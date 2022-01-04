namespace InterTwitter.Models
{
    public class HashtagModel : IEntityBase
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int TweetsCount { get; set; }
    }
}
