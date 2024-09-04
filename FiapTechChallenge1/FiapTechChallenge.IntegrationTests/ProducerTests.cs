using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Producer.Controllers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
namespace FiapTechChallenge.IntegrationTests;
public class ContactControllerTests
{
    private readonly Mock<IBus> _mockBus;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly ContactProducerController _controller;

    public ContactControllerTests()
    {
        _mockBus = new Mock<IBus>();
        _mockConfiguration = new Mock<IConfiguration>();

        _controller = new ContactProducerController(_mockBus.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task CreateContact_SendsMessageToQueue_WhenModelStateIsValid()
    {
        // Arrange
        var personDto = new PersonRequestByDDDDto
        {
            // Preencha com dados de teste apropriados
            Name = "João da Silva",
            CPF = "12345678901",
            Email = "teste@teste.com",
            Birthday = DateTime.Now,
            Phones = new List<PhoneRequestByDDDDto>
                    {
                        new PhoneRequestByDDDDto
                        {
                            DDDNumber = 11,
                            PhoneNumber = "999999999",
                            PhoneType = "Celular"
                        }
                    }

        };
        var nomeFila = "fila_fiap_techChallenge";

        _mockConfiguration.Setup(c => c.GetSection("MassTransit")["NomeFila"]).Returns(nomeFila);

        var mockEndpoint = new Mock<ISendEndpoint>();
        _mockBus.Setup(b => b.GetSendEndpoint(It.IsAny<Uri>())).ReturnsAsync(mockEndpoint.Object);

        // Act
        var result = await _controller.CreateContact(personDto);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        _mockBus.Verify(bus => bus.GetSendEndpoint(It.Is<Uri>(uri => uri.ToString() == $"queue:{nomeFila}")), Times.Once);
        mockEndpoint.Verify(endpoint => endpoint.Send(personDto, default), Times.Once);
    }
}
