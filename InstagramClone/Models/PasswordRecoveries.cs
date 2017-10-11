using System;
using System.Collections.Generic;

namespace InstagramClone.Models
{
    public partial class PasswordRecoveries
    {
        public int Id { get; set; }
        public int User { get; set; }
        public string Password { get; set; }
        public string VerifyId { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public Users UserNavigation { get; set; }
    }
}
