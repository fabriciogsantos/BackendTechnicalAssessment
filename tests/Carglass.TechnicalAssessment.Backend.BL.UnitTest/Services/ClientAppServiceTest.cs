using System.Linq.Expressions;
using AutoMapper;
using Carglass.TechnicalAssessment.Backend.BL.Clients;
using Carglass.TechnicalAssessment.Backend.BL.Clients.Converter;
using Carglass.TechnicalAssessment.Backend.BL.Interfaces;
using Carglass.TechnicalAssessment.Backend.Core.Dtos;
using Carglass.TechnicalAssessment.Backend.DL.Interfaces.Repositories;
using Carglass.TechnicalAssessment.Backend.Dtos.Clients;
using Carglass.TechnicalAssessment.Backend.Entities;
using Carglass.TechnicalAssessment.Backend.Seeds.Clients;
using Microsoft.Extensions.Logging;

namespace Carglass.TechnicalAssessment.Backend.BL.UnitTest.Services
{
    public class ClientAppServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ClientAppService>> _logger;
        private readonly Mock<IValidator<ClientDto>> _clientValidator;
        private readonly Mock<IClientInMemoryRepository> _clientInMemoryRepository;
        private readonly IClientAppService _clientAppService;

        public ClientAppServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ClientProfileConverter());
            });

            _mapper = config.CreateMapper();
            _logger = new Mock<ILogger<ClientAppService>>();
            _clientValidator = new Mock<IValidator<ClientDto>>();
            _clientInMemoryRepository = new Mock<IClientInMemoryRepository>();
          
            _clientAppService = new ClientAppService(_clientInMemoryRepository.Object, _clientValidator.Object, _mapper,_logger.Object);
        }


        [Fact]
        public void When_GetAll_Success()
        {
            // Arrange
            _clientInMemoryRepository.Setup(x => x.GetAll(It.IsAny<Pagination>())).Returns(ClientSeed.Generate(100)); 
            
            // Act
            var result = _clientAppService.GetAll(new Pagination{Page = 1, Take = 100});
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<IEnumerable<ClientDto>>>(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Result);
            Assert.Equal(100, result.Result?.Count());
        }

        [Fact]
        public void When_GetAll_Fail()
        {
            // Arrange
            _clientInMemoryRepository.Setup(x => x.GetAll(It.IsAny<Pagination>())).Returns(Enumerable.Empty<Client>());
            
            // Act
            var result = _clientAppService.GetAll(new Pagination { Page = 1, Take = 100 });
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<IEnumerable<ClientDto>>>(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Result);
            Assert.False(result.Result?.Any());
        }

        [Fact]
        public void When_GetById_Success()
        {
            // Arrange
            var client = ClientSeed.Generate(1).First();
            _clientInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(client);

            // Act
            var result = _clientAppService.GetById(client.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<ClientDto>>(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Result);
            Assert.True(result.Result?.Id == client.Id);
        }

        [Fact]
        public void When_GetById_Fail()
        {
            // Arrange
            _clientInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(null as Client);

            // Act
            var result = _clientAppService.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<ClientDto>>(result);
            Assert.True(result.HasErrors);
            Assert.Null(result.Result);
        }
        
        [Fact]
        public void When_Create_Success()
        {
            // Arrange
            var clientDto = _mapper.Map<ClientDto>(ClientSeed.Generate(1).First());
            clientDto.Id = 0;
            _clientInMemoryRepository.Setup(x => x.Create(It.IsAny<Client>())).Returns(true);
            _clientInMemoryRepository.Setup(x => x.GetNextId()).Returns(1);
            _clientInMemoryRepository.Setup(x => x.Search(It.IsAny<Expression<Func<Client,bool>>>())).Returns(Enumerable.Empty<Client>());
            _clientValidator.Setup(x => x.Validate(It.IsAny<ClientDto>())).Returns(new FluentValidation.Results.ValidationResult());
            
            // Act
            var result = _clientAppService.Create(clientDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<ClientDto>>(result);
            Assert.False(result.HasErrors);
            Assert.True(result.Result?.Id == 1);
        }

        [Fact]
        public void When_Create_Fail()
        {
            // Arrange
            var client = ClientSeed.Generate(1).First();
            var clientDto = _mapper.Map<ClientDto>(client);
            clientDto.Id = 0;
            
            _clientInMemoryRepository.Setup(x => x.GetNextId()).Returns(1);
            _clientInMemoryRepository.Setup(x => x.Search(It.IsAny<Expression<Func<Client, bool>>>())).Returns(new List<Client>{ client });
            _clientValidator.Setup(x => x.Validate(It.IsAny<ClientDto>())).Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var result = _clientAppService.Create(clientDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<ClientDto>>(result);
            Assert.True(result.HasErrors);
            Assert.Null(result.Result);
            _clientInMemoryRepository.Verify(x => x.Create(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public void When_Update_Success()
        {
            // Arrange
            var client = ClientSeed.Generate(1).First();
            var clientDto = _mapper.Map<ClientDto>(client);
            
            _clientInMemoryRepository.Setup(x => x.Update(It.IsAny<Client>())).Returns(true);
            _clientInMemoryRepository.Setup(x => x.Search(It.IsAny<Expression<Func<Client, bool>>>())).Returns(Enumerable.Empty<Client>());
            _clientInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(client);
            _clientValidator.Setup(x => x.Validate(It.IsAny<ClientDto>())).Returns(new FluentValidation.Results.ValidationResult());
            
            // Act
            var result = _clientAppService.Update(clientDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult>(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void When_Update_Fail()
        {
            // Arrange
            var client = ClientSeed.Generate(1).First();
            var clientDto = _mapper.Map<ClientDto>(client);
            
            _clientInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(client);
            _clientInMemoryRepository.Setup(x => x.Search(It.IsAny<Expression<Func<Client, bool>>>())).Returns(new List<Client> { client });
            _clientValidator.Setup(x => x.Validate(It.IsAny<ClientDto>())).Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var result = _clientAppService.Update(clientDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult>(result);
            Assert.True(result.HasErrors);
            _clientInMemoryRepository.Verify(x => x.Update(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public void When_Delete_Success()
        {
            // Arrange
            var client = ClientSeed.Generate(1).First();

            _clientInMemoryRepository.Setup(x => x.Delete(It.IsAny<Client>())).Returns(true);
            _clientInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(client);

            // Act
            var result = _clientAppService.Delete(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult>(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void When_Delete_Fail()
        {
            // Arrange
            _clientInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(null as Client);

            // Act
            var result = _clientAppService.Delete(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult>(result);
            Assert.True(result.HasErrors);
            _clientInMemoryRepository.Verify(x => x.Delete(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public void When_Delete_ExeFail()
        {
            // Arrange
            _clientInMemoryRepository.Setup(x => x.Delete(It.IsAny<Client>())).Returns(true);
            _clientInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(null as Client);

            // Act
            var result = _clientAppService.Delete(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult>(result);
            Assert.True(result.HasErrors);
        }
    }
}
