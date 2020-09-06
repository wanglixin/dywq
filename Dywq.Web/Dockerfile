#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry.cn-hangzhou.aliyuncs.com/newbe36524/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM registry.cn-hangzhou.aliyuncs.com/newbe36524/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Dywq.Web/Dywq.Web.csproj", "Dywq.Web/"]
COPY ["Dywq.Infrastructure/Dywq.Infrastructure.csproj", "Dywq.Infrastructure/"]
COPY ["Dywq.Infrastructure.Core/Dywq.Infrastructure.Core.csproj", "Dywq.Infrastructure.Core/"]
COPY ["Dywq.Domain/Dywq.Domain.csproj", "Dywq.Domain/"]
COPY ["Dywq.Domain.Abstractions/Dywq.Domain.Abstractions.csproj", "Dywq.Domain.Abstractions/"]
RUN dotnet restore "Dywq.Web/Dywq.Web.csproj"
COPY . .
WORKDIR "/src/Dywq.Web"
RUN dotnet build "Dywq.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dywq.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dywq.Web.dll"]