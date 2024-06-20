using AutoMapper;
using Events;
using MassTransit;
using MongoDB.Entities;

namespace UserService;
public class UserUpdatedConsumer : IConsumer<UserUpdated>
{
    private readonly IMapper _mapper;

    public UserUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<UserUpdated> userUpdated)
    {
        Console.WriteLine("Consuming user update " + userUpdated.Message._id);

        var user = _mapper.Map<User>(userUpdated.Message);

        var result = await DB.Update<User>().Match(user => user.ID == userUpdated.Message._id).ModifyOnly(
            user => new
            {
                user.FirstName,
                user.MiddleName,
                user.LastName,
                user.Email,
                user.ContactNumber,
                user.Age,
                user.Country,
                user.State,
                user.City,
                user.PinCode,
                user.UserType,
                user.Skills,
                user.UpdatedAt,

            }, user).ExecuteAsync();
        if (!result.IsAcknowledged)
            throw new MessageException(typeof(UserUpdated), "Problem updating mongodb");
    }
}