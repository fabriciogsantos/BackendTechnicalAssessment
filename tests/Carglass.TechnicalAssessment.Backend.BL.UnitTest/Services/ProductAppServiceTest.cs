using AutoMapper;
using Carglass.TechnicalAssessment.Backend.BL.Interfaces;
using Carglass.TechnicalAssessment.Backend.BL.Products;
using Carglass.TechnicalAssessment.Backend.BL.Products.Converter;
using Carglass.TechnicalAssessment.Backend.Core.Dtos;
using Carglass.TechnicalAssessment.Backend.DL.Interfaces.Repositories;
using Carglass.TechnicalAssessment.Backend.Dtos.Products;
using Carglass.TechnicalAssessment.Backend.Entities;
using Carglass.TechnicalAssessment.Backend.Seeds.Products;
using Microsoft.Extensions.Logging;

namespace Carglass.TechnicalAssessment.Backend.BL.UnitTest.Services
{
    public class ProductAppServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ProductAppService>> _logger;
        private readonly Mock<IValidator<ProductDto>> _productValidator;
        private readonly Mock<IProductInMemoryRepository> _productInMemoryRepository;
        private readonly IProductAppService _productAppService;

        public ProductAppServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductProfileConverter());
            });

            _mapper = config.CreateMapper();
            _logger = new Mock<ILogger<ProductAppService>>();
            _productValidator = new Mock<IValidator<ProductDto>>();
            _productInMemoryRepository = new Mock<IProductInMemoryRepository>();
          
            _productAppService = new ProductAppService(_productInMemoryRepository.Object, _productValidator.Object, _mapper,_logger.Object);
        }


        [Fact]
        public void When_GetAll_Success()
        {
            // Arrange
            _productInMemoryRepository.Setup(x => x.GetAll(It.IsAny<Pagination>())).Returns(ProductSeed.Generate(100)); 
            
            // Act
            var result = _productAppService.GetAll(new Pagination{Page = 1, Take = 100});
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<IEnumerable<ProductDto>>>(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Result);
            Assert.Equal(100, result.Result?.Count());
        }

        [Fact]
        public void When_GetAll_Fail()
        {
            // Arrange
            _productInMemoryRepository.Setup(x => x.GetAll(It.IsAny<Pagination>())).Returns(Enumerable.Empty<Product>());
            
            // Act
            var result = _productAppService.GetAll(new Pagination { Page = 1, Take = 100 });
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<IEnumerable<ProductDto>>>(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Result);
            Assert.False(result.Result?.Any());
        }

        [Fact]
        public void When_GetById_Success()
        {
            // Arrange
            var product = ProductSeed.Generate(1).First();
            _productInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(product);

            // Act
            var result = _productAppService.GetById(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<ProductDto>>(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Result);
            Assert.True(result.Result?.Id == product.Id);
        }

        [Fact]
        public void When_GetById_Fail()
        {
            // Arrange
            _productInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(null as Product);

            // Act
            var result = _productAppService.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<ProductDto>>(result);
            Assert.True(result.HasErrors);
            Assert.Null(result.Result);
        }
        
        [Fact]
        public void When_Create_Success()
        {
            // Arrange
            var productDto = _mapper.Map<ProductDto>(ProductSeed.Generate(1).First());
            productDto.Id = 0;
            _productInMemoryRepository.Setup(x => x.Create(It.IsAny<Product>())).Returns(true);
            _productInMemoryRepository.Setup(x => x.GetNextId()).Returns(1);
            _productInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(null as Product);
            _productValidator.Setup(x => x.Validate(It.IsAny<ProductDto>())).Returns(new FluentValidation.Results.ValidationResult());
            
            // Act
            var result = _productAppService.Create(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<ProductDto>>(result);
            Assert.False(result.HasErrors);
            Assert.True(result.Result?.Id == 1);
        }

        [Fact]
        public void When_Create_Fail()
        {
            // Arrange
            var product = ProductSeed.Generate(1).First();
            var productDto = _mapper.Map<ProductDto>(product);
            productDto.Id = 0;
            
            _productInMemoryRepository.Setup(x => x.GetNextId()).Returns(1);
            _productInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(product);
            _productValidator.Setup(x => x.Validate(It.IsAny<ProductDto>())).Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var result = _productAppService.Create(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult<ProductDto>>(result);
            Assert.True(result.HasErrors);
            Assert.Null(result.Result);
            _productInMemoryRepository.Verify(x => x.Create(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void When_Update_Success()
        {
            // Arrange
            var product = ProductSeed.Generate(1).First();
            var productDto = _mapper.Map<ProductDto>(product);
            
            _productInMemoryRepository.Setup(x => x.Update(It.IsAny<Product>())).Returns(true);
            _productInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(product);
            _productValidator.Setup(x => x.Validate(It.IsAny<ProductDto>())).Returns(new FluentValidation.Results.ValidationResult());
            
            // Act
            var result = _productAppService.Update(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult>(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void When_Update_Fail()
        {
            // Arrange
            var product = ProductSeed.Generate(1).First();
            var productDto = _mapper.Map<ProductDto>(product);
            
            _productInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(null as Product);
            _productValidator.Setup(x => x.Validate(It.IsAny<ProductDto>())).Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var result = _productAppService.Update(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult>(result);
            Assert.True(result.HasErrors);
            _productInMemoryRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void When_Delete_Success()
        {
            // Arrange
            var product = ProductSeed.Generate(1).First();

            _productInMemoryRepository.Setup(x => x.Delete(It.IsAny<Product>())).Returns(true);
            _productInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(product);

            // Act
            var result = _productAppService.Delete(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult>(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void When_Delete_Fail()
        {
            // Arrange
            _productInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(null as Product);

            // Act
            var result = _productAppService.Delete(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult>(result);
            Assert.True(result.HasErrors);
            _productInMemoryRepository.Verify(x => x.Delete(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void When_Delete_ExeFail()
        {
            // Arrange
            _productInMemoryRepository.Setup(x => x.Delete(It.IsAny<Product>())).Returns(true);
            _productInMemoryRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(null as Product);

            // Act
            var result = _productAppService.Delete(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OperationResult>(result);
            Assert.True(result.HasErrors);
        }
    }
}
