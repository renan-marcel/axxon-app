using Moq;
using ProdAbs.Application.Features.TiposDocumento.Commands;
using ProdAbs.Application.Features.TiposDocumento.Handlers;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;

namespace ProdAbs.UnitTests
{
    public class CriarTipoDocumentoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ShouldReturnSuccessResult()
        {
            // Arrange
            var repositoryMock = new Mock<ITipoDeDocumentoRepository>();
            var handler = new CriarTipoDocumentoCommandHandler(repositoryMock.Object);
            var command = new CriarTipoDocumentoCommand
            {
                Nome = "Test Document Type",
                Campos = new System.Collections.Generic.List<string> { "Field1", "Field2" }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(command.Nome, result.Value.Nome);
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<TipoDocumento>()), Times.Once);
        }
    }
}