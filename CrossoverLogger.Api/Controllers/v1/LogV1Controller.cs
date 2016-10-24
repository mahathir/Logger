using CrossoverLogger.Api.Filters;
using CrossoverLogger.Api.Model.v1;
using CrossoverLogger.Commons.Translation;
using CrossoverLogger.DTO;
using CrossoverLogger.IBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrossoverLogger.Api.Controllers.v1
{
    [CrossoverAuthFilter]
    public class LogV1Controller : ApiController
    {
        private readonly ILogService logService;

        public LogV1Controller(ILogService logService)
        {
            this.logService = logService;
        }

        public IHttpActionResult Post([FromBody]LogV1Request requestBody)
        {
            if (requestBody == null)
            {
                return BadRequest(MessagesResx.JsonInvalid);
            }

            if((User.Identity as CrossoverAuthIdentity).ApplicationId != requestBody.ApplicationId)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            var response = new LogV1Response() { Success = true };
            var log = new Log
            {
                ApplicationId = requestBody.ApplicationId,
                Level = requestBody.Level,
                Logger = requestBody.Logger,
                Message = requestBody.Message
            };

            var result = this.logService.Create(log);

            if (result.ErrorMessages.Count > 0)
            {
                return BadRequest(result.ErrorSummary);
            }

            return Ok(response);
        }
    }
}