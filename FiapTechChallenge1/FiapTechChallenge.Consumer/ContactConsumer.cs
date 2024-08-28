using MassTransit;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.DTOs.RequestsDto;

namespace FiapTechChallenge.Consumer;

public class ContactConsumer : IConsumer<PersonRequestByDDDDto>
{
    private readonly IPersonService _personService;

    public ContactConsumer(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task Consume(ConsumeContext<PersonRequestByDDDDto> context)
    {
        var contact = context.Message;
        Console.WriteLine($"Contact received: {contact.Name} - {contact.Email}");
        await _personService.CreateContact(contact);
        return;
    }
}
