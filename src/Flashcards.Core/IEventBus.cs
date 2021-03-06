﻿using System;

namespace Flashcards.Core
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;

        void Subscribe(Action<IEvent> processMessage);

        void Unsubscribe();
    }
}