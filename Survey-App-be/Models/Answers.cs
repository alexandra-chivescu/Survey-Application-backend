namespace Survey_App_be.Models
{
    public class Answers
    {
        public int Id { get; set; }
        public int Question_id { get; set; }
        public string Answer { get; set; }
        public int NoResponses { get; set; }
    }
}
