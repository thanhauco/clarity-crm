using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Clarity.Core.Interfaces;
using Clarity.Core.Models;
using Clarity.Services;

namespace Clarity.Services.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _service = new CustomerService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetCustomerAsync_ReturnsCustomer_WhenExists()
        {
            // Arrange
            var customerId = 1;
            var expected = new Customer
            {
                Id = customerId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(customerId))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetCustomerAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Email, result.Email);
        }

        [Fact]
        public async Task GetCustomerAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _service.GetCustomerAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateCustomerAsync_CreatesAndReturnsCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com"
            };

            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Customer>()))
                .ReturnsAsync((Customer c) =>
                {
                    c.Id = 1;
                    return c;
                });

            // Act
            var result = await _service.CreateCustomerAsync(customer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Jane", result.FirstName);
        }

        [Fact]
        public async Task CreateCustomerAsync_ThrowsException_WhenFirstNameEmpty()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "",
                LastName = "Smith",
                Email = "jane@example.com"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CreateCustomerAsync(customer));
        }

        [Fact]
        public async Task DeleteCustomerAsync_ReturnsTrue_WhenDeleted()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteCustomerAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(999))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteCustomerAsync(999);

            // Assert
            Assert.False(result);
        }
    }

    public class LeadServiceTests
    {
        private readonly Mock<ILeadRepository> _mockLeadRepo;
        private readonly Mock<ICustomerRepository> _mockCustomerRepo;
        private readonly LeadService _service;

        public LeadServiceTests()
        {
            _mockLeadRepo = new Mock<ILeadRepository>();
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _service = new LeadService(_mockLeadRepo.Object, _mockCustomerRepo.Object);
        }

        [Fact]
        public async Task CreateLeadAsync_SetsStatusToNew()
        {
            // Arrange
            var lead = new Lead
            {
                FirstName = "Test",
                LastName = "Lead",
                Email = "test@example.com"
            };

            _mockLeadRepo.Setup(r => r.CreateAsync(It.IsAny<Lead>()))
                .ReturnsAsync((Lead l) => l);

            // Act
            var result = await _service.CreateLeadAsync(lead);

            // Assert
            Assert.Equal(LeadStatus.New, result.Status);
        }

        [Fact]
        public async Task ConvertToCustomerAsync_CreatesCustomerAndUpdatesLead()
        {
            // Arrange
            var lead = new Lead
            {
                Id = 1,
                FirstName = "Convert",
                LastName = "Me",
                Email = "convert@example.com",
                Company = "Test Corp"
            };

            _mockLeadRepo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(lead);

            _mockCustomerRepo.Setup(r => r.CreateAsync(It.IsAny<Customer>()))
                .ReturnsAsync((Customer c) =>
                {
                    c.Id = 10;
                    return c;
                });

            _mockLeadRepo.Setup(r => r.UpdateAsync(It.IsAny<Lead>()))
                .ReturnsAsync((Lead l) => l);

            // Act
            var customer = await _service.ConvertToCustomerAsync(1);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(10, customer.Id);
            Assert.Equal(lead.Email, customer.Email);
        }
    }
}
