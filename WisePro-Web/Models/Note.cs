using System.ComponentModel.DataAnnotations.Schema;

namespace WisePro_Web.Models
{
    public class Note
    {
        public int note_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int? user_id { get; set; }

    }
}
