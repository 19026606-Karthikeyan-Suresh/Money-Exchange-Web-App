namespace FYP.Models
{
    public class Enquiry
    {
        public int enquiry_id { get; set; }
        public string visitor_email_address { get; set; }
        public string description { get; set; }
        public string enquiry_date { get; set; }
        public byte status { get; set; }
        public string answered_by { get; set; }
        public byte deleted { get; set; }
        public string deleted_by { get; set; }
    }
}
