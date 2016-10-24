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
    public class AuthV1Controller : ApiController
    {
        private readonly ITokenService tokenService;

        public AuthV1Controller(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public IHttpActionResult Post()
        {
            IEnumerable<string> headerValues;
            var authHeader = string.Empty;
            if (Request.Headers.TryGetValues("Authorization", out headerValues))
            {
                authHeader = headerValues.FirstOrDefault();
            }
            if (string.IsNullOrWhiteSpace(authHeader)) return StatusCode(HttpStatusCode.Unauthorized);

            var splitHeader = authHeader.Split(':');
            if(splitHeader.Length != 2) return StatusCode(HttpStatusCode.Unauthorized);

            var applicationId = splitHeader[0];
            var applicationSecret = splitHeader[1];
            
            var result = this.tokenService.GenerateNewToken(applicationId, applicationSecret);
            if (result.ErrorMessages.Count > 0)
            {
                if (result.ErrorMessages.ContainsKey(nameof(Application.ApplicationId)) ||
                    result.ErrorMessages.ContainsKey(nameof(Application.Secret)))
                {
                    return Unauthorized();
                }
                else
                {
                    return BadRequest(result.ErrorSummary);
                }
            }

            var response = new AuthV1Response() { Token = result.Result.AccessToken };
            return Ok(response);
        }
    }
}