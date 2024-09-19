using MassTransit;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.DTOs.RequestsDto;

namespace FiapTechChallenge.Consumer;

public class CreateContactConsumer : IConsumer<PersonRequestByDDDDto>
{
    private readonly IPersonService _personService;

    public CreateContactConsumer(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task Consume(ConsumeContext<PersonRequestByDDDDto> context)
    {
        var contact = context.Message;
        Console.WriteLine($"Contact received: {contact.Name} - {contact.Email}");
        await _personService.CreateContact(contact);
    }
}
