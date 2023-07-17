using System.ComponentModel.DataAnnotations;

namespace BankingSystemAPI.Models
{
    public class UserAccountDto
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }

        public IList<AccountDto> Accounts { get; set; }
    }
}
