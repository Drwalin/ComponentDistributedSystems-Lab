FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /home/LibrarySystem/Library.NotificationService2
COPY . /home/LibrarySystem/Library.NotificationService2
RUN ls -l
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /home/LibrarySystem/Library.NotificationService2/out /app
ENTRYPOINT ["dotnet", "Library.NotificationService2.dll"]

