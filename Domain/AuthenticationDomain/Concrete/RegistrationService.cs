using Database.Entities;
using Database.Repository;
using Domain.AuthenticationDomain.Exceptions;
using Domain.AuthenticationDomain.Helpers;
using Domain.AuthenticationDomain.Models.Requests;
using Domain.AuthenticationDomain.Models.Responses;

namespace Domain.AuthenticationDomain.Concrete;

internal class RegistrationService : IRegistrationService
{
    private readonly ICommandRepository<User> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrationService(ICommandRepository<User> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisterUserResponse> Register(RegisterUserRequest request)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            Password = PasswordHasher.HashPassword(request.Password)
        };

        var result = await _repository.AddAsync(user);

        if (!await _unitOfWork.SaveChanges()) throw new RegistrationException();

        return new RegisterUserResponse(result.Id);
    }
}