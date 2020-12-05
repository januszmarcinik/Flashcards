﻿using System;
using System.Linq;

namespace Flashcards.Core
{
    public class Mediator : IMediator
    {
        private readonly IDependencyResolver _dependencyResolver;

        public Mediator(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public Result Command<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _dependencyResolver.ResolveOrDefault<ICommandHandler<TCommand>>();
            if (handler == null)
            {
                throw new InvalidOperationException($"Command of type '{command.GetType()}' has not registered handler.");
            }

            return handler.Handle(command);
        }

        public Result<TResult> Query<TResult>(IQuery<TResult> query)
        {
            return (Result<TResult>)GetType()
                .GetMethods()
                .First(x => x.Name == "Query" && x.GetGenericArguments().Length == 2)
                .MakeGenericMethod(query.GetType(), typeof(TResult))
                .Invoke(this, new object[] { query });
        }

        public Result<TResult> Query<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var handler = _dependencyResolver.ResolveOrDefault<IQueryHandler<TQuery, TResult>>();
            if (handler == null)
            {
                throw new InvalidOperationException($"Query of type '{query.GetType()}' has not registered handler.");
            }

            return handler.Handle(query);
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = _dependencyResolver.ResolveMany<IEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                handler.Handle(@event);
            }
        }
    }
}
