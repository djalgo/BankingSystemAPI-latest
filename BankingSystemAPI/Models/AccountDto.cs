using System.ComponentModel.DataAnnotations;

namespace BankingSystemAPI.Models
{
    /// <summary>
    /// User's bank Account Information Dto
    /// </summary>
    public class AccountDto
    {
        public int userId { get; }
        [Required]
        public decimal Balance { get; set; }

    }
}

