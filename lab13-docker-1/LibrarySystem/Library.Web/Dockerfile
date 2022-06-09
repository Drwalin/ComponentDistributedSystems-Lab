FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /home/LibrarySystem/Library.Web
COPY ./* /home/LibrarySystem/Library.Web/
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /home/LibrarySystem/Library.Web/out /app
ENTRYPOINT ["dotnet", "Library.Web.dll"]

