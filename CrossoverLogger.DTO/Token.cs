namespace CrossoverLogger.DTO
{

    public partial class Token
    {
        public string AccessToken { get; set; }

        public string ApplicationId { get; set; }

        public virtual Application Application { get; set; }
    }
}
