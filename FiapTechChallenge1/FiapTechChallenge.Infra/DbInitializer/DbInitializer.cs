using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FiapTechChallenge.Infra.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                // Verifica se o banco de dados já foi criado
                context.Database.EnsureCreated();

                // Verifica se já existem dados
                if (context.People.Any())
                {
                    // Se já existirem dados, não faz nada
                    return;
                }

                // Adicione aqui o código para inserir os dados iniciais
                // Exemplo:
                context.People.Add(new Person
                {
                    // props
                });

                // Salve as alterações no banco de dados
                context.SaveChanges();
            }
        }
    }
}
