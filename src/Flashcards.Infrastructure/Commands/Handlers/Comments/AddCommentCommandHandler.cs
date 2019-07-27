﻿using Flashcards.Core.Extensions;
using Flashcards.Infrastructure.Commands.Models.Comments;
using System;
using Flashcards.Core;
using Flashcards.Domain.Repositories;

namespace Flashcards.Infrastructure.Commands.Handlers.Comments
{
    internal class AddCommentCommandHandler : ICommandHandler<AddCommentCommand>
    {
        private readonly ICommentsRepository _commentsRepository;

        public AddCommentCommandHandler(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        public Result Handle(AddCommentCommand command)
        {
            if (command.Id.IsEmpty())
            {
                command.Id = Guid.NewGuid();
            }

            _commentsRepository.Add(command.CardId, command.UserId, command.Text);

            return Result.Ok();
        }
    }
}
