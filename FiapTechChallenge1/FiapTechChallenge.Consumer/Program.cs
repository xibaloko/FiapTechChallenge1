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
        services.AddScoped<IDbInitializer, DbInitializer>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
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
                    e.PrefetchCount = 1;
                    e.ConfigureConsumer<UpdateContactConsumer>(context);
                    e.ConfigureConsumer<CreateContactConsumer>(context);
                    e.ConfigureConsumer<DeleteContactConsumer>(context);

                    // Configurando Retry
                    //.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                    // Configurando Circuit Breaker
                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 2;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<CreateContactConsumer>();
            x.AddConsumer<UpdateContactConsumer>();
            x.AddConsumer<DeleteContactConsumer>();
        });



    })
    .Build();

host.Run();