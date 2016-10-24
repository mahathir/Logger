namespace CrossoverLogger.BusinessLogic
{
    using IBusinessLogic;
    using System.Collections.Generic;
    using System.Text;

    public class ServiceResult<TEntity> : IServiceResult<TEntity>
    {
        private Dictionary<string, string> _errorMessages = new Dictionary<string, string>();
        public IDictionary<string, string> ErrorMessages
        {
            get
            {
                return _errorMessages;
            }
        }

        public TEntity Result { get; set; }

        public string ErrorSummary
        {
            get
            {
                StringBuilder errorSummary = new StringBuilder(string.Empty);
                foreach (var message in ErrorMessages)
                {
                    errorSummary.AppendLine(message.Value);
                }

                return errorSummary.ToString();
            }
        }
    }
}
