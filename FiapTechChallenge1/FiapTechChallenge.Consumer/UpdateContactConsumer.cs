using MassTransit;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.DTOs.RequestsDto;

namespace FiapTechChallenge.Consumer;

public class UpdateContactConsumer : IConsumer<UpdateRequest>
{
    private readonly IPersonService _personService;

    public UpdateContactConsumer(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task Consume(ConsumeContext<UpdateRequest> context)
    {
        var contact = context.Message;
        Console.WriteLine($"Contact received: {contact.Name} - {contact.Email}");
        await _personService.UpdateContact(contact.Id, contact);

    }
}
