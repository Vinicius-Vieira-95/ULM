# Stage 1 - Base
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ARG APP_NAME
LABEL app=${APP_NAME}

# Stage 2 - Build
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["UlmApi.Application/UlmApi.Application.csproj", "UlmApi.Application/"]
COPY ["UlmApi.Service/UlmApi.Service.csproj", "UlmApi.Service/"]
COPY ["UlmApi.Infra.Data/UlmApi.Infra.Data.csproj", "UlmApi.Infra.Data/"]
COPY ["UlmApi.Domain/UlmApi.Domain.csproj", "UlmApi.Domain/"]
COPY ["UlmApi.Infra.CrossCutting/UlmApi.Infra.CrossCutting.csproj", "UlmApi.Infra.CrossCutting/"]
RUN dotnet restore "UlmApi.Application/UlmApi.Application.csproj"
COPY . .
WORKDIR "/src/UlmApi.Application"
RUN dotnet build "UlmApi.Application.csproj" -c Release -o /app/build

# Stage 3 - Publish
FROM build AS publish
RUN dotnet publish "UlmApi.Application.csproj" -c Release -o /app/publish

# Stage 4 - Deploy
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UlmApi.Application.dll"]