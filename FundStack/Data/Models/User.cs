using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundStack.Data.Models
{
    [NotMapped]
    public class User : IdentityUser
    {
        public Portfolio Portfolio { get; set; } = new Portfolio();
    }
}
