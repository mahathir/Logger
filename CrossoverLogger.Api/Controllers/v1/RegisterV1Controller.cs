using CrossoverLogger.Api.Model.v1;
using System;
using System.Web.Http;
using CrossoverLogger.Commons.Translation;
using CrossoverLogger.DTO;
using CrossoverLogger.IBusinessLogic;

namespace CrossoverLogger.Api.Controllers.v1
{
    public class RegisterV1Controller : ApiController
    {
        private readonly IApplicationService appService;

        public RegisterV1Controller(IApplicationService appService)
        {
            this.appService = appService;
        }

        public IHttpActionResult Post([FromBody]RegisterV1Request requestBody)
        {
            if (requestBody == null)
            {
                return BadRequest(MessagesResx.JsonInvalid);
            }

            var result = this.appService.Create(new Application { DisplayName = requestBody.DisplayName });

            if(result.ErrorMessages.Count > 0)
            {
                return BadRequest(result.ErrorSummary);
            }

            var response = new RegisterV1Response()
            {
                ApplicationId = result.Result.ApplicationId,
                ApplicationSecret = result.Result.Secret,
                DisplayName = result.Result.DisplayName
            };
            return Ok(response);
        }
    }
}