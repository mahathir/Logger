namespace CrossoverLogger.DTO
{
    public partial class Log
    {
        public long LogId { get; set; }
        
        public string Logger { get; set; }
        
        public string Level { get; set; }
        
        public string Message { get; set; }
        
        public string ApplicationId { get; set; }

        public virtual Application Application { get; set; }
    }
}
