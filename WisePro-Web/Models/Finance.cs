using System.ComponentModel.DataAnnotations.Schema;

namespace WisePro_Web.Models
{
    public class Finance
    {
        public int finance_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string type_expenses { get; set; }
        public int amount { get; set; }
        public DateTime date { get; set; }
        public int? user_id { get; set; }

        
    }
}
