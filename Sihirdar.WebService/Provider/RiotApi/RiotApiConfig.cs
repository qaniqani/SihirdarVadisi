namespace Sihirdar.WebService.Provider.RiotApi
{
    public class RiotApiConfig
    {
        public string ApiKey { get; set; }
        public int RateLimitPer10S { get; set; }
        public int RateLimitPer10M { get; set; }
    }
}
