using System.ComponentModel.DataAnnotations;
using static FundStack.Data.DataConstants;


namespace FundStack.Models.Portfolio
{
    public class AddMoneyViewModel
    {
        public string? UserId { get; set; }

        [Range(MinMoneyAdd, double.MaxValue, ErrorMessage = "The added money cannot be less than $1")]
        public decimal Money { get; set; }
    }
}

