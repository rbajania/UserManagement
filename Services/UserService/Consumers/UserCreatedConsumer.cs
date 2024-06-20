using AutoMapper;
using Events;
using MassTransit;
using MongoDB.Entities;

namespace UserService;
public class UserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly IMapper _mapper;

    public UserCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<UserCreated> userCreated)
    {
        Console.WriteLine("Consuming user created " + userCreated.Message._id);

        var user = _mapper.Map<User>(userCreated.Message);

        await user.SaveAsync();
    }
}