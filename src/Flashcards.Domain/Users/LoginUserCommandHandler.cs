﻿using System;
using Flashcards.Core;
using Flashcards.Domain.Extensions;

namespace Flashcards.Domain.Users
{
    internal class LoginUserCommandHandler : ICommandHandler<LoginUserCommand>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        private readonly ICacheService _cache;
        private readonly EncryptionService _encryptionService;

        public LoginUserCommandHandler(IUsersRepository usersRepository, ITokenService tokenService, ICacheService cache, EncryptionService encryptionService)
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
            _cache = cache;
            _encryptionService = encryptionService;
        }

        public Result Handle(LoginUserCommand command)
        {
            Result HandleError() => Result.Fail("Invalid email or password.");

            var user = _usersRepository.GetByEmail(command.Email);
            if (user == null)
            {
                return HandleError();
            }

            var hash = _encryptionService.GetHash(command.Password, user.Salt);
            if (user.Password != hash)
            {
                return HandleError();
            }

            var jwt = _tokenService.CreateToken(user.Id, user.Email, user.Role);
            
            _cache.SetJwt(command.TokenId, jwt);

            return Result.Ok();
        }
    }
}
