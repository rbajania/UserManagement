using Events;
using MassTransit;
using MongoDB.Entities;

namespace UserService;
public class UserDeletedConsumer : IConsumer<UserDeleted>
{
    public async Task Consume(ConsumeContext<UserDeleted> userDeleted)
    {
        Console.WriteLine("Consuming user delete " + userDeleted.Message.Id);

        var result = await DB.DeleteAsync<User>(userDeleted.Message.Id);

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(UserDeleted), "Problem deleting User");
    }

}