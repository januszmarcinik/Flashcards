﻿using System;
using Flashcards.Core;
using Microsoft.Extensions.Logging;

namespace Flashcards.Domain.Cards
{
    public class CardEditedEventHandler : IEventHandler<CardEditedEvent>
    {
        private readonly ISqlCardsRepository _sqlCardsRepository;
        private readonly INoSqlCardsRepository _noSqlCardsRepository;
        private readonly ILogger<CardEditedEventHandler> _logger;

        public CardEditedEventHandler(
            ISqlCardsRepository sqlCardsRepository, 
            INoSqlCardsRepository noSqlCardsRepository,
            ILogger<CardEditedEventHandler> logger)
        {
            _sqlCardsRepository = sqlCardsRepository;
            _noSqlCardsRepository = noSqlCardsRepository;
            _logger = logger;
        }
        
        public void Handle(CardEditedEvent @event)
        {
            var card = _sqlCardsRepository.GetById(@event.CardId);
            if (card == null)
            {
                _logger.LogError("Card with Id {CardId} does not exist", @event.CardId);
                return;
            }

            var dto = card.ToDto(Guid.Empty, Guid.NewGuid());
            _noSqlCardsRepository.Update(dto);
        }
    }
}