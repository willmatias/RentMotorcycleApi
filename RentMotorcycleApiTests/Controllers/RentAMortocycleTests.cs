using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RentMotorcycle.Business.Interface.Service;
using RentMotorcycle.Business.Models.Documents;
using RentMotorcycle.Business.Models.Param;
using RentMotorcycle.Business.Services;
using RentMotorcycle.Data.Context;

namespace RentMotorcycleApi.Controllers.Tests
{
    [TestClass()]
    public class RentAMortocycleTests
    {
        
        private readonly Mock<ILogger<RentAMortocycle>> _loggerMock;
        private readonly Mock<IRentAMotorcycleService> _rentAMotorcycleService;        
        private readonly Mock<IUserService> _userService;
        private readonly RentAMortocycle _rentAMortocycleTests;
        public RentAMortocycleTests()
        {
            _loggerMock = new Mock<ILogger<RentAMortocycle>>();
            _rentAMotorcycleService = new Mock<IRentAMotorcycleService>();
            _userService = new Mock<IUserService>();

            _rentAMortocycleTests = new RentAMortocycle(
                _loggerMock.Object,
                _rentAMotorcycleService.Object);
        }

        [TestMethod()]
        public async Task PostRentAMotorcycle_WhenIsOk_ExpectOkResult()
        {
            var rentAMotorcycleParam = new RentAMotorcycleParam { DataFim = DateTime.Now.AddDays(5), DataInicio = DateTime.Now.AddDays(1), DataPrevisao = DateTime.Now.AddMonths(2), Dias = 10 };

            (string mensagem, bool status) = ("Aluguel cadastrado com sucesso, valor do aluguel é 100", true);
            
            _userService.Setup(x => x.GetUserById(It.IsAny<string>())).ReturnsAsync(new RentMotorcycle.Business.Models.Documents.User { TipoCnh = TipoCNH.A, Cnh = "121231", Cnpj = "123131313212" });
            
            _rentAMotorcycleService.Setup(x => x.InsertNewRentAMotorcycle(It.IsAny<RentAMotorcycleParam>())).ReturnsAsync((mensagem, status));

            var result = await _rentAMortocycleTests.PostRentAMotorcycle(rentAMotorcycleParam).ConfigureAwait(false);

            var okResult = result as OkObjectResult;

            Assert.AreEqual(200, okResult?.StatusCode);            
        }

        [TestMethod()]
        public async Task PostRentAMotorcycle_InvalidData_ReturnsBadRequest()
        {
            var rentAMotorcycleParam = new RentAMotorcycleParam { DataFim = DateTime.Now.AddDays(5), DataInicio = DateTime.Now, DataPrevisao = DateTime.Now.AddMonths(2), Dias = 10 };

            (string mensagem, bool status) = ("DataInicio tem que ser maior que a data atual", false);

            _userService.Setup(x => x.GetUserById(It.IsAny<string>())).ReturnsAsync(new RentMotorcycle.Business.Models.Documents.User { TipoCnh = TipoCNH.A, Cnh = "121231", Cnpj = "123131313212" });

            _rentAMotorcycleService.Setup(x => x.InsertNewRentAMotorcycle(It.IsAny<RentAMotorcycleParam>())).ReturnsAsync((mensagem, status));

            var result = await _rentAMortocycleTests.PostRentAMotorcycle(rentAMotorcycleParam).ConfigureAwait(false);

            var nokResult = result as BadRequestObjectResult;

            Assert.AreEqual(400, nokResult?.StatusCode);
        }

        [TestMethod()]
        public async Task PostRentAMotorcycle_InvalidCategoryUser_ReturnsBadRequest()
        {
            var rentAMotorcycleParam = new RentAMotorcycleParam { DataFim = DateTime.Now.AddDays(5), DataInicio = DateTime.Now, DataPrevisao = DateTime.Now.AddMonths(2), Dias = 10 };


            _userService.Setup(x => x.GetUserById(It.IsAny<string>())).ReturnsAsync(new RentMotorcycle.Business.Models.Documents.User { TipoCnh = TipoCNH.B, Cnh = "121231", Cnpj = "123131313212" });
            (string mensagem, bool status) = ("Usuário fora da catagoria", false);

            _rentAMotorcycleService.Setup(x => x.InsertNewRentAMotorcycle(It.IsAny<RentAMotorcycleParam>())).ReturnsAsync((mensagem, status));

            var result = await _rentAMortocycleTests.PostRentAMotorcycle(rentAMotorcycleParam).ConfigureAwait(false);

            var nokResult = result as BadRequestObjectResult;

            Assert.AreEqual(400, nokResult?.StatusCode);
        }
    }
}