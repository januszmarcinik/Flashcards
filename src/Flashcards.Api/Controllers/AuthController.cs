﻿using System;
using Flashcards.Core;
using Flashcards.Domain;
using Flashcards.Domain.Extensions;
using Flashcards.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : ApiController
    {
        private readonly ICacheService _cache;

        public AuthController(IMediator mediator, IEventBus eventBus, ICacheService cache) 
            : base(mediator, eventBus)
        {
            _cache = cache;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginUserCommand command)
        {
            command.SetTokenId(Guid.NewGuid());
            JwtDto GetValueOnSuccess() => _cache.GetJwt(command.TokenId);
            return Dispatch(command, GetValueOnSuccess);
        }
    }
}
