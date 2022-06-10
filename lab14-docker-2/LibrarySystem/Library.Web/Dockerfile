FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /home
COPY . /home
RUN ls -l
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /home/out .
COPY --from=build /home/wwwroot ./wwwroot
COPY --from=build /home/Views ./Views
COPY --from=build /home/Utilities ./Utilities
RUN ls -l
ENTRYPOINT ["dotnet", "Library.Web.dll"]

