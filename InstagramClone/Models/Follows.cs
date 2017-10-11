using System;
using System.Collections.Generic;

namespace InstagramClone.Models
{
    public partial class Follows
    {
        public int Id { get; set; }
        public int? FollowingUser { get; set; }
        public int? FollowedUser { get; set; }
        public DateTime? FollowDate { get; set; }

        public Users FollowedUserNavigation { get; set; }
        public Users FollowingUserNavigation { get; set; }
    }
}
