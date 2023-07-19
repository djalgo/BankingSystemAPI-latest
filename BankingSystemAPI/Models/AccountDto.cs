using System.ComponentModel;
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
        [Range(1, 10000, ErrorMessage = "Amount deposited must not be greater than 10000$ in a single transaction.")]
        [DefaultValue(0)]
        public decimal Balance { get; set; }

    }
}

