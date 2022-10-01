using FluentValidation;
using InstagramClone.Api.Extensions;
using InstagramClone.Application.Helpers;
using InstagramClone.Application.Sieve;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.EmailSender.Services;
using InstagramClone.Infrastructure.DAL;
using InstagramClone.Infrastructure.DAL.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sieve.Services;
using System;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<InstagramCloneDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection")));

ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-US");

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilters>();
builder.Services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();

builder.Services.AddScoped<IRepository<UserProfile>, EntityRepository<UserProfile>>();

builder.Services.AddScoped<IRepository<UserPost>, EntityRepository<UserPost>>();
builder.Services.AddScoped<IRepository<PostComment>, EntityRepository<PostComment>>();
builder.Services.AddScoped<IRepository<PostLike>, EntityRepository<PostLike>>();

builder.Services.AddTransient<RabbitMQHelper>();
builder.Services.AddHostedService<ConsumeEmailSenderMessagingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.ConfigureCustomExceptionMiddleware();

app.Run();
