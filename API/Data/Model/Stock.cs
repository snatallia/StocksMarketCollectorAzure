using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace API.Data.Model
{
    public class Stock
    {
        public Guid Id { get; set; }

        [DisplayName("Symb")]
        [Required]
        public string Symbol { get; set; }
        public string DisplaySymbol { get; set; }
        
        [DefaultValue("USD")]
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Mic { get; set; }
        public string Type { get; set; }

        [DefaultValue(false)]
        public bool IsSP500 { get; set; }
    }
}
