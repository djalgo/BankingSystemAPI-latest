using System.ComponentModel.DataAnnotations;

namespace BankingSystemAPI.Models
{
    /// <summary>
    /// User's bank Account Information
    /// </summary>
    public class Account
    {
        
        public string AccountUserId { get; set; }
        public string AccountNumber { get; set; }

        
        public decimal Balance { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

    }
}

