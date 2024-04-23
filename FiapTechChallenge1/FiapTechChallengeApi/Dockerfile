# Use o SDK do .NET para construir a imagem
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie o arquivo .csproj e restaure quaisquer dependências (por exemplo, via NuGet)
COPY ["FiapTechChallengeApi.csproj", "./"]
RUN dotnet restore "FiapTechChallengeApi.csproj"

# Copie os arquivos restantes do seu projeto e construa sua aplicação
COPY . .
RUN dotnet build "FiapTechChallengeApi.csproj" -c Release -o /app/build

# Publique a aplicação
FROM build AS publish
RUN dotnet publish "FiapTechChallengeApi.csproj" -c Release -o /app/publish

# Gere a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FiapTechChallengeApi.dll"]
