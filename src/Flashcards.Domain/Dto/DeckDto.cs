﻿using System;

namespace Flashcards.Domain.Dto
{
    public class DeckDto
    {
        public DeckDto(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
    }
}
