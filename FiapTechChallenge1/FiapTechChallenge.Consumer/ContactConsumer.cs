using FiapTechChallenge.Producer.DTOs;
using MassTransit;
namespace FiapTechChallenge.Consumer
{
    public class ContactConsumer :IConsumer<PersonRequestByDDDDto>
    {
        public Task Consume(ConsumeContext<PersonRequestByDDDDto> context)
        {
            var contact = context.Message;
            Console.WriteLine($"Contact received: {contact.Name} - {contact.Email}");
            return Task.CompletedTask;
        }
    }
}
