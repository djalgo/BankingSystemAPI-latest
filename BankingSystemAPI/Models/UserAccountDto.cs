using System.ComponentModel.DataAnnotations;

namespace BankingSystemAPI.Models
{
    public class UserAccountDto
    {
        [Required]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public IList<AccountDto> Accounts { get; set; }
    }
}
