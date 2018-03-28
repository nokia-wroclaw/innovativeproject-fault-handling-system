FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /Fault-handling-system

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /Fault-handling-system
COPY --from=build-env /Fault-handling-system/out .
ENTRYPOINT ["dotnet", "Fault-handling-system.dll"]
