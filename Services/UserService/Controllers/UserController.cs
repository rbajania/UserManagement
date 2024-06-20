using UserService.DTOs;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Events;
using MassTransit;
using MongoDB.Entities;
using UserService.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Polly;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly MongoDBContext _mongoContext;
        public UserController(IMapper mapper, IPublishEndpoint publishEndpoint, MongoDBContext mongoDBContext)
        {
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _mongoContext = mongoDBContext;
        }
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _mongoContext.Users
            .OrderBy(x => x.UpdatedAt)
            .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(ObjectId id)
        {
            var foundUser = await _mongoContext.Users.FirstOrDefaultAsync(x => x.ID == id);

            if (foundUser == null) return NotFound();

            return _mapper.Map<UserDto>(foundUser);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {

            try
            {
                var user = _mapper.Map<User>(createUserDto);

                _mongoContext.Users.Add(user);

                var newUser = _mapper.Map<UserDto>(user);

                await _publishEndpoint.Publish(_mapper.Map<UserCreated>(newUser));

                var result = await _mongoContext.SaveChangesAsync() > 0;

                if (!result) return BadRequest("Could not save changes to the DB");

                return CreatedAtAction(nameof(GetUserById),
                    new { user.ID }, newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(ObjectId id, UpdateUserDto updateUserDto)
        {

            var user = await _mongoContext.Users.FirstOrDefaultAsync(x => x.ID == id);

            if (user == null) return NotFound();

            user.FirstName = updateUserDto.FirstName ?? user.FirstName;
            user.MiddleName = updateUserDto.MiddleName ?? user.MiddleName;
            user.LastName = updateUserDto.LastName ?? user.LastName;
            user.Email = updateUserDto.Email ?? user.Email;
            user.ContactNumber = updateUserDto.ContactNumber == 0 ? user.ContactNumber : updateUserDto.ContactNumber;
            user.Age = updateUserDto.Age == 0 ? user.Age : updateUserDto.Age;
            user.City = updateUserDto.City ?? user.City;
            user.State = updateUserDto.State ?? user.State;
            user.Country = updateUserDto.Country ?? user.Country;
            user.PinCode = updateUserDto.PinCode == 0 ? user.PinCode : updateUserDto.PinCode;
            user.UserType = updateUserDto.UserType ?? user.UserType;
            user.Skills = updateUserDto.Skills ?? user.Skills;
            user.UpdatedAt = DateTime.UtcNow;

            await _publishEndpoint.Publish(_mapper.Map<UserUpdated>(user));

            var result = await _mongoContext.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest("Problem saving changes");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(ObjectId id)
        {
            var user = await _mongoContext.Users.FindAsync(id);

            if (user == null) return NotFound();

            _mongoContext.Users.Remove(user);

            await _publishEndpoint.Publish<UserDeleted>(new { Id = user.ID });

            var result = await _mongoContext.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Could not update DB");

            return Ok();
        }
    }
}
