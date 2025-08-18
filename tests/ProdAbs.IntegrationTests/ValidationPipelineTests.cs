using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using ProdAbs.Application.Features.TiposDocumento.Commands;
using System.Collections.Generic;

namespace ProdAbs.IntegrationTests
{
    public class ValidationPipelineTests : IClassFixture<WebApplicationFactory<Presentation.Api.Controllers.TiposDocumentoController>>
    {
        private readonly HttpClient _client;

        public ValidationPipelineTests(WebApplicationFactory<Presentation.Api.Controllers.TiposDocumentoController> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CriarTipoDocumento_WithInvalidCommand_ShouldReturnBadRequest()
        {
            // Arrange
            var command = new CriarTipoDocumentoCommand
            {
                Nome = "", // Invalid name
                Campos = new List<string> { "Test" }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/TiposDocumento", command);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
