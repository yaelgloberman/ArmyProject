namespace ArmyExe2.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public Username user { get; set; }
        public List<Username> listUser { get; set; }
        public CoronaDetails corona { get; set; }
        public List<CoronaDetails> listCoronaDetails { get; set; }
        public List<int>daysSick { get; set; }
        public int NotVaccinatedCount { get; set; }

    }
}
