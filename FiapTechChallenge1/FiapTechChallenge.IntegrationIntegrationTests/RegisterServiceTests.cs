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
            // Aqui voc� pode usar o ServiceProvider para obter inst�ncias de servi�os e contextos
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Verificar se os dados foram inicializados corretamente
                Assert.True(context.Regions.Any());
            }
        }
    }
}