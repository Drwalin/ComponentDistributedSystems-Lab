# !/bin/bash

docker network create -d bridge ksrlabbridge

docker run -p 5672:5672 -p 15672:15672 --network=ksrlabbridge -d --name rabbitmq rabbitmq:management
docker run -p 92:80 -d -it --name notif mcr.microsoft.com/dotnet/core/sdk:2.2 bash -c "cd /home/LibrarySystem/Library.NotificationService2 && dotnet restore && dotnet publish -c Release -o out && cd out && dotnet Library.NotificationService2.dll"
docker cp LibrarySystem notif:/home/
docker start notif
sleep 20
docker commit notif notif_rel_img
docker stop notif
docker run -d -p  92:80 --network=ksrlabbridge --name notif_rel notif_rel_img bash -c "cd /home/LibrarySystem/Library.NotificationService2/out && dotnet Library.NotificationService2.dll"


