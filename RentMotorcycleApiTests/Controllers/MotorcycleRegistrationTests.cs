using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using RentMotorcycle.Business.Interface.Context;
using RentMotorcycle.Business.Interface.Service;
using RentMotorcycle.Business.Models.Documents;
using RentMotorcycle.Business.Models.Param;
using RentMotorcycle.Business.Services;
using RentMotorcycle.Data.Context;
using System.Linq.Expressions;
using Xunit;

namespace RentMotorcycleApi.Controllers.Tests
{

    [TestClass()]
    public class MotorcycleRegistrationTests
    {
        private readonly Mock<IRabbitService> _rabbitService;
        private readonly Mock<IMotorsService> _motorService;        
        private readonly Mock<ILogger<MotorcycleRegistration>> _loggerMock;
        private readonly MotorcycleRegistration _motorcycleRegistrationTests;        

        public MotorcycleRegistrationTests()
        {
            _rabbitService = new Mock<IRabbitService>();
            _motorService = new Mock<IMotorsService>();            
            _loggerMock = new Mock<ILogger<MotorcycleRegistration>>();
            
            _motorcycleRegistrationTests = new MotorcycleRegistration(
                _rabbitService.Object,
                _motorService.Object,
                _loggerMock.Object);
        }      


        [TestMethod()]
        public async Task PostRecordMotorcycle_WhenIsOk_ExpectOkResult()
        {
            MotorcycleModel motorcycleModel = new MotorcycleModel { Ano = 1988, Identificador = 564564566, Modelo = "SUZUKI", Placa = "RDZ0R52"};

            var placa = "RDDA123";

            _motorService.Setup(repo => repo.IsUniquePlaca(It.IsAny<string>())).ReturnsAsync(false);

            var result = await _motorcycleRegistrationTests.PostRecordMotorcycle(motorcycleModel).ConfigureAwait(false);

            var okResult = result as OkObjectResult;

            Assert.AreEqual(200, okResult?.StatusCode);
        }

        [TestMethod()]
        public async Task PostRecordMotorcycle_WhenIsNOk_ExpectNOkResult()
        {
            MotorcycleModel motorcycleModel = new MotorcycleModel { Ano = 1988, Identificador = 564564566, Modelo = "SUZUKI", Placa = "RDZ0R52" };

            var placa = "RDDA123";

            _motorService.Setup(repo => repo.IsUniquePlaca(It.IsAny<string>())).ReturnsAsync(true);

            var result = await _motorcycleRegistrationTests.PostRecordMotorcycle(motorcycleModel).ConfigureAwait(false);

            var nokResult = result as BadRequestObjectResult;

            Assert.AreEqual(400, nokResult?.StatusCode);
        }

        [TestMethod()]
        public async Task UpdateMotor_ReturnsUpdateOkResult()
        {
            // Arrange
            var motorId = "60616d6a356324bf5f5d1c3a"; // ID de exemplo
            var placa = "ABC1234"; // Placa de exemplo
            var updateResult = new UpdateResult.Acknowledged(1, 1, "60616d6a356324bf5f5d1c3a");

            _motorService.Setup(repo => repo.UpdateMotor(It.IsAny<string>(), It.IsAny<string>()))
                          .ReturnsAsync(updateResult);

            // Act
            var result = await _motorcycleRegistrationTests.PutMotorcycle(motorId, placa);

            var okResult = result as OkObjectResult;

            // Assert
            Assert.AreEqual(200, okResult?.StatusCode);
        }

        [TestMethod()]
        public async Task UpdateMotor_ReturnsUpdateNokResult()
        {
            // Arrange
            var motorId = "60616d6a356324bf5f5d1c3a"; // ID de exemplo
            var placa = "ABC1234"; // Placa de exemplo
            var updateResult = new UpdateResult.Acknowledged(0, 0, "60616d6a356324bf5f5d1c3a"); // Resultado de exemplo

            
            _motorService.Setup(repo => repo.UpdateMotor(It.IsAny<string>(), It.IsAny<string>()))
                          .ReturnsAsync(updateResult);

            // Act
            var result = await _motorcycleRegistrationTests.PutMotorcycle(motorId, placa).ConfigureAwait(false);

            var nokResult = result as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(400, nokResult?.StatusCode);
        }

    }
}