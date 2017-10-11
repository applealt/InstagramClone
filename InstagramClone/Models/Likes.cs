using System;
using System.Collections.Generic;

namespace InstagramClone.Models
{
    public partial class Likes
    {
        public int Id { get; set; }
        public int User { get; set; }
        public int Post { get; set; }
        public DateTime? LikeDate { get; set; }

        public Posts PostNavigation { get; set; }
        public Users UserNavigation { get; set; }
    }
}
