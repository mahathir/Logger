namespace CrossoverLogger.DTO
{
    using System.Collections.Generic;
    
    public partial class Application
    {
        public Application()
        {
            Logs = new HashSet<Log>();
            RateLimit = new HashSet<RateLimit>();
            Tokens = new HashSet<Token>();
        }
        
        public string ApplicationId { get; set; }

        public string DisplayName { get; set; }

        public string Secret { get; set; }

        public virtual ICollection<Log> Logs { get; set; }

        public virtual ICollection<RateLimit> RateLimit { get; set; }

        public virtual ICollection<Token> Tokens { get; set; }
    }
}
