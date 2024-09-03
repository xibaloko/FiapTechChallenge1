using MassTransit;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.DTOs.RequestsDto;

namespace FiapTechChallenge.Consumer;

public class DeleteContactConsumer : IConsumer<DeletePersonRequest>
{
    private readonly IPersonService _personService;

    public DeleteContactConsumer(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task Consume(ConsumeContext<DeletePersonRequest> context)
    {
        await _personService.DeleteContact(context.Message.Id);
    }
}