﻿using AutoMapper;
using Flashcards.Core.Exceptions;
using Flashcards.Infrastructure.Services.Abstract.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flashcards.Domain.Dto;
using Flashcards.Infrastructure.DataAccess;
using Flashcards.Infrastructure.Extensions;

namespace Flashcards.Infrastructure.Services.Concrete.Queries
{
    internal class CommentsQueryService : ICommentsQueryService
    {
        private readonly EFContext _dbContext;
        private readonly IMapper _mapper;

        public CommentsQueryService(EFContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<CommentDto>> GetByCardAsync(Guid cardId)
        {
            var card = await _dbContext.Cards.FindAndEnsureExistsAsync(cardId, ErrorCode.CardDoesNotExist);
            var comments = card.Comments.OrderByDescending(x => x.Date);
            return _mapper.Map<List<CommentDto>>(comments);
        }

        public async Task<CommentDto> GetByIdAsync(Guid id)
            => _mapper.Map<CommentDto>(await _dbContext.Comments.FindAndEnsureExistsAsync(id, ErrorCode.InvalidCommentText));
    }
}
