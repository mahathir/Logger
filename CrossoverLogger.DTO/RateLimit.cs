namespace CrossoverLogger.DTO
{
    using System;

    public partial class RateLimit
    {
        public long Id { get; set; }
        
        public string ApplicationId { get; set; }
        
        public long LimitTime { get; set; }

        public int LimitRate { get; set; }

        public int SuspendTime { get; set; }

        public int RemainingRate { get; set; }

        public DateTime? StartingTime { get; set; }

        public DateTime? StartingSuspend { get; set; }

        public virtual Application Application { get; set; }
    }
}
