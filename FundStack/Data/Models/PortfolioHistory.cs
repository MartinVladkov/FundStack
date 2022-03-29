using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundStack.Data.Models
{
    public class PortfolioHistory
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PortfolioValue { get; set; }

        public DateTime SnapshotDate { get; set; }

        public string PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
    }
}
