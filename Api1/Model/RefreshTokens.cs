using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api1.Model
{
    public class RefreshTokens 
    {
        [Key]
       public int Id { get; set; }

        public string Token { get; set; }

        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }

        public DateTime? Revoked { get; set; }

        public bool IsExpired => DateTime.Now >= Expires;

        public bool IsActive => Revoked==null && !IsExpired;


        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
