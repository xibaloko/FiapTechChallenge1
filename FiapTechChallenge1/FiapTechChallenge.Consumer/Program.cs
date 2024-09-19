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
using Prometheus;

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

        services.AddHostedService<MetricsHostedService>();

    })
    .Build();

host.Run();


public class MetricsHostedService : IHostedService
{
    private IHostApplicationLifetime _lifetime;
    private KestrelMetricServer _metricServer;

    public MetricsHostedService(IHostApplicationLifetime lifetime)
    {
        _lifetime = lifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Inicia um servidor HTTP na porta 1234 expondo as métricas
        _metricServer = new KestrelMetricServer(port: 4002);
        _metricServer.Start();

        _lifetime.ApplicationStopping.Register(() =>
        {
            _metricServer.Stop();
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _metricServer?.Stop();
        return Task.CompletedTask;
    }
}