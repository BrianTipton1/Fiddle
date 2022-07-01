#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
RUN apt update
RUN apt install ffmpeg python3-pip -y
RUN apt install libopus0 opus-tools libopus-dev libsodium-dev -y
#RUN apt install curl tar autoconf automake libtool gcc make -y
### Might need to update versions at some point
#RUN curl https://archive.mozilla.org/pub/opus/opus-1.3.1.tar.gz -o opus.tar.gz
#RUN tar xvzf opus.tar.gz 
#RUN ls && cd opus-1.3.1 && ./configure && make && make install && cd ..
#RUN curl https://download.libsodium.org/libsodium/releases/LATEST.tar.gz -o libsodium.tar.gz
#RUN tar xvzf libsodium.tar.gz 
#RUN cd libsodium-stable && ./configure && make && make check && make install && cd ..
RUN python3 -m pip install -U yt-dlp
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SatansFiddle/SatansFiddle.csproj", "SatansFiddle/"]
RUN dotnet restore "SatansFiddle/SatansFiddle.csproj"
COPY . .
WORKDIR "/src/SatansFiddle"
RUN dotnet build "SatansFiddle.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SatansFiddle.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/SatansFiddle/appsettings.json .
ENTRYPOINT ["dotnet", "SatansFiddle.dll"]