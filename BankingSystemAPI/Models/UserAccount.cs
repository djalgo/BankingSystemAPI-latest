
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemAPI.Models
{
    public class UserAccount
    {
        [Key]
        public string userId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        
        public IList<Account> Accounts { get; set; }
    }
}
