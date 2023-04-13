namespace WisePro_Web.Models
{
    public class User_Task
    {
        public int tasks_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime creation_date { get; set; }
        public string priority { get; set; }
        public string activity { get; set; }
        public DateTime? expiration_date { get; set; }
        public string status { get; set; }
        public int? user_id { get; set; }

       
    }
}
