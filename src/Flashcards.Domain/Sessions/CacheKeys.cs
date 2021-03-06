﻿using System;

namespace Flashcards.Domain.Sessions
{
    internal static class CacheKeys
    {
        public static string GetSessionStateKey(Guid userId, string deck)
            => $"session-state-{userId}-{deck}";

        public static string GetSessionCardsKey(Guid sessionId)
            => $"session-cards-{sessionId}";
    }
}
