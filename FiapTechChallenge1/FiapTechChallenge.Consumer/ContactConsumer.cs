using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiapTechChallenge.Domain.DTOs.RequestsDto;
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
