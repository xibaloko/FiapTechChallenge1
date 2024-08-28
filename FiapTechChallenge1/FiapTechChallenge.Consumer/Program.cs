using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.AppService.Services;
using FiapTechChallenge.Consumer;
using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.DbInitializer;
using FiapTechChallenge.Infra.Interfaces;
using FiapTechChallenge.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using MassTransit.RabbitMqTransport;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var fila = configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;
        services.AddHostedService<Worker>();

        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbInitializer, DbInitializer>();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }, ServiceLifetime.Scoped);

        
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });

                cfg.ReceiveEndpoint(fila, e =>
                {
                    //e.Consumer<ContactConsumer>(context);  // Resolva o consumidor a partir do contexto
                    e.ConfigureConsumer<ContactConsumer>(context);  // Resolva o consumidor a partir do contexto
                    //e.ConfigureConsumer(context, typeof(ContactConsumer));
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<ContactConsumer>();
        });



    })
    .Build();

host.Run();