﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using InstagramClone.Infrastructure.DAL.Context;

namespace InstagramClone.Api.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using var appContext = scope.ServiceProvider.GetRequiredService<InstagramCloneDbContext>();
                appContext.Database.Migrate();
            }

            return host;
        }
    }
}