#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app/EmailSender

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["InstagramClone.EmailSender/InstagramClone.EmailSender.csproj", "InstagramClone.EmailSender/"]
COPY ["InstagramClone.Domain/InstagramClone.Domain.csproj", "InstagramClone.Domain/"]
COPY ["InstagramClone.Infrastructure/InstagramClone.Infrastructure.csproj", "InstagramClone.Infrastructure/"]
COPY ["InstagramClone.Application/InstagramClone.Application.csproj", "InstagramClone.Application/"]
COPY ["InstagramClone.Api/InstagramClone.Api.csproj", "InstagramClone.Api/"]
RUN dotnet restore "InstagramClone.EmailSender/InstagramClone.EmailSender.csproj"
COPY . .
WORKDIR "/src/InstagramClone.EmailSender"
RUN dotnet build "InstagramClone.EmailSender.csproj" -c Release -o /app/build/EmailSender

FROM build AS publish
RUN dotnet publish "InstagramClone.EmailSender.csproj" -c Release -o /app/publish/EmailSender

FROM base AS final
WORKDIR /app/EmailSender
COPY --from=publish /app/publish/EmailSender .
ENTRYPOINT ["dotnet", "InstagramClone.EmailSender.dll"]