using System.ComponentModel.DataAnnotations;
using System.Data;

namespace TriInkom.Entities
{
    public enum EApplicationStatus
    {
        Published,
        Unpublished
    }
    public class Application
    {
        [Key]
        public string Number { get; set; } 
        public EApplicationStatus Status { get; set; }
        public decimal Amount { get; set; }
        public int TermValue { get; set; } 
        public decimal InterestValue {  get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

    }
}
