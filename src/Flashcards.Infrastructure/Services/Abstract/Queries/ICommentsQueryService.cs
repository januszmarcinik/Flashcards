﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flashcards.Domain.Dto;

namespace Flashcards.Infrastructure.Services.Abstract.Queries
{
    public interface ICommentsQueryService
    {
        Task<List<CommentDto>> GetByCardAsync(Guid cardId);
        Task<CommentDto> GetByIdAsync(Guid id);
    }
}
