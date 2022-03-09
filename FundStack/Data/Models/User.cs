using Microsoft.AspNetCore.Identity;

namespace FundStack.Data.Models
{
    public class User : IdentityUser
    {
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
    }
}
