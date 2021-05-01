﻿using Flashcards.Domain.Images;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flashcards.Infrastructure.WindowsStorage
{
    public static class Extensions
    {
        public static IServiceCollection AddWindowsStorage(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddSettings<WindowsStorageSettings>(configuration)
                .AddScoped<IImagesStorage, WindowsImagesStorage>();
    }
}