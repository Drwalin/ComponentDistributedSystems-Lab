FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /home/LibrarySystem/Library.WebApi
COPY . /home/LibrarySystem/Library.WebApi
RUN ls -l
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /home/LibrarySystem/Library.WebApi/out /app
ENTRYPOINT ["dotnet", "Library.WebApi.dll"]

