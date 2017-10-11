using System;
using System.Collections.Generic;

namespace InstagramClone.Models
{
    public partial class Posts
    {
        public Posts()
        {
            Likes = new HashSet<Likes>();
        }

        public int Id { get; set; }
        public int User { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public string HashTag { get; set; }
        public int LikeCount { get; set; }
        public int FeedbackCount { get; set; }
        public DateTime CreatedDate { get; set; }

        public Users UserNavigation { get; set; }
        public ICollection<Likes> Likes { get; set; }
    }
}
