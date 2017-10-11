using System;
using System.Collections.Generic;

namespace InstagramClone.Models
{
    public partial class Users
    {
        public Users()
        {
            FollowsFollowedUserNavigation = new HashSet<Follows>();
            FollowsFollowingUserNavigation = new HashSet<Follows>();
            Likes = new HashSet<Likes>();
            PasswordRecoveries = new HashSet<PasswordRecoveries>();
            Posts = new HashSet<Posts>();
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageProfile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PostsCount { get; set; }
        public int? FollowersCount { get; set; }
        public int? FollowingCount { get; set; }
        public DateTime? CreatedDate { get; set; }

        public ICollection<Follows> FollowsFollowedUserNavigation { get; set; }
        public ICollection<Follows> FollowsFollowingUserNavigation { get; set; }
        public ICollection<Likes> Likes { get; set; }
        public ICollection<PasswordRecoveries> PasswordRecoveries { get; set; }
        public ICollection<Posts> Posts { get; set; }
    }
}
