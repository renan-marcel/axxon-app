using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ProdAbs.IntegrationTests
{
    public class ApiAuthenticationTests : IClassFixture<WebApplicationFactory<Presentation.Api.Controllers.TiposDocumentoController>>
    {
        private readonly HttpClient _client;

        public ApiAuthenticationTests(WebApplicationFactory<Presentation.Api.Controllers.TiposDocumentoController> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTiposDocumento_WithoutToken_ShouldReturnUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/TiposDocumento");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}