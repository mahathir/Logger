using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossoverLogger.Api.Controllers.v1;
using Moq;
using CrossoverLogger.IBusinessLogic;
using CrossoverLogger.IDataAccess;
using Ninject;
using CrossoverLogger.DataAccess.EF;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using CrossoverLogger.BusinessLogic;
using CrossoverLogger.Api.Model.v1;
using System.Web.Http.Results;
using CrossoverLogger.DTO;
using CrossoverLogger.Commons.Translation;
using System.Data.Entity.Validation;

namespace CrossoverLogger.Api.Tests.Controller
{
    /// <summary>
    /// Summary description for RegisterControllerTests
    /// </summary>
    [TestClass]
    public class RegisterV1ControllerTests
    {
        private readonly MoqMockingKernel kernel;

        public RegisterV1ControllerTests()
        {
            this.kernel = new MoqMockingKernel();
            // Repository
            this.kernel.Bind<CrossoverLoggerContext>().ToMock().InSingletonScope();
            this.kernel.Bind<IApplicationRepository>().ToMock().InSingletonScope();
            this.kernel.Bind<IRateLimitRepository>().ToMock().InSingletonScope();
            this.kernel.Bind<ILogRepository>().ToMock().InSingletonScope();
            this.kernel.Bind<ITokenRepository>().ToMock().InSingletonScope();

            // Service
            this.kernel.Bind<IApplicationService>().To<ApplicationService>();
            this.kernel.Bind<IRateLimitService>().To<RateLimitService>();
            this.kernel.Bind<ILogService>().To<LogService>();
            this.kernel.Bind<ITokenService>().To<TokenService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.kernel.Reset();
        }

        [TestMethod]
        public void Register_WithValidDisplayName_ReturnOKStatusAndValidAppObject()
        {
            // Arrange
            var appService = this.kernel.Get<IApplicationService>();
            var controller = new RegisterV1Controller(appService);
            var request = new RegisterV1Request { DisplayName = "Test" };

            // Act
            var rawResult = controller.Post(request);

            // Assert
            Assert.IsInstanceOfType(rawResult, typeof(OkNegotiatedContentResult<RegisterV1Response>));
            var okResult = rawResult as OkNegotiatedContentResult<RegisterV1Response>;
            Assert.IsNotNull(okResult.Content);
            Assert.IsFalse(string.IsNullOrWhiteSpace(okResult.Content.ApplicationId));
            Assert.AreEqual(32, okResult.Content.ApplicationId.Length);
            Assert.IsFalse(string.IsNullOrWhiteSpace(okResult.Content.ApplicationSecret));
            Assert.AreEqual(25, okResult.Content.ApplicationSecret.Length);
            Assert.IsFalse(string.IsNullOrWhiteSpace(okResult.Content.DisplayName));
            Assert.AreEqual(request.DisplayName, okResult.Content.DisplayName);
        }

        [TestMethod]
        public void Register_WithNullDisplayName_ReturnBadRequest()
        {
            // Arrange
            var appService = this.kernel.Get<IApplicationService>();
            var controller = new RegisterV1Controller(appService);
            var request = new RegisterV1Request ();

            // Act
            var rawResult = controller.Post(request);

            // Assert
            Assert.IsInstanceOfType(rawResult, typeof(BadRequestErrorMessageResult));
            var badResult = rawResult as BadRequestErrorMessageResult;
            Assert.IsFalse(string.IsNullOrWhiteSpace(badResult.Message));
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine(string.Format(MessagesResx._CantBe_, nameof(Application.DisplayName), CommonsResx.Empty));
            Assert.AreEqual(strBuilder.ToString(), badResult.Message);
        }

        [TestMethod]
        public void Register_DisplayNameMoreThan25Char_ReturnBadRequest()
        {
            // Arrange
            var repo = this.kernel.GetMock<IApplicationRepository>();
            repo.Setup(a => a.SaveChanges()).Callback(() => { throw new DbEntityValidationException("Error"); });

            var appService = this.kernel.Get<IApplicationService>();
            var controller = new RegisterV1Controller(appService);
            var request = new RegisterV1Request { DisplayName = "ThisIsAplicaitionThatHaveVeryVeryLongName" };

            // Act
            var rawResult = controller.Post(request);

            // Assert
            Assert.IsInstanceOfType(rawResult, typeof(BadRequestErrorMessageResult));
            var badResult = rawResult as BadRequestErrorMessageResult;
            Assert.IsFalse(string.IsNullOrWhiteSpace(badResult.Message));
        }
    }
}
