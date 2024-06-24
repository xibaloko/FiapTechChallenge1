using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.IntegrationTests;
using Microsoft.Extensions.DependencyInjection;

namespace FiapTechChallenge.IntegrationIntegrationTests
{
    public class RegisterServiceTests : TestBase
    {
        [Fact]
        public void TestDatabaseInitialization()
        {
            // Aqui você pode usar o ServiceProvider para obter instâncias de serviços e contextos
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Verificar se os dados foram inicializados corretamente
                Assert.True(context.Regions.Any());
            }
        }
    }
}