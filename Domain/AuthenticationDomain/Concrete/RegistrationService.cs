using System.Net;
using Database.Entities;
using Database.Repositories;
using Domain.AuthenticationDomain.Helpers;
using Domain.AuthenticationDomain.Models.Requests;
using Domain.AuthenticationDomain.Models.Responses;
using Domain.SharedDomain;

namespace Domain.AuthenticationDomain.Concrete;

internal class RegistrationService : IRegistrationService
{
    private DomainError RegistrationError => new("Unable to register user", HttpStatusCode.BadRequest);

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
            Password = PasswordHasher.HashPassword(request.Password),
        };

        var result = await _repository.AddAsync(user);

        if (!await _unitOfWork.SaveChanges()) throw RegistrationError;

        return new RegisterUserResponse(result.Id);
    }
}