# Fault Handling System

## Administrator Guide

### Preparing Docker environment - Ubuntu 16.04 LTS

First, you have to have Docker installed on your system.

As a post-installation step, add yourself to `docker` group:

`sudo usermod -aG docker <username>`

and log out and log in again for the change to take effect.

### Running in a Docker container - Ubuntu 16.04 LTS

  1. Open a command prompt and navigate to the project folder
     `Fault-handling-system/`
  1. Use the following commands to build your Docker image:
```
    $ docker build -t fault-handling-system .
```
  1. It is recommended to start both PostgreSQL
     and the application using `docker-compose`:
```
    $ docker-compose -f fhs.yml up
```

Now you can navigate your browser to http://localhost:8080/
(or http://localhost:10000/ for PostgreSQL administration).

### Connecting to PostgreSQL from docker-compose

Add the following environment variable to `fault-handling-system` container:

    ConnectionStrings__AzureConnection: Server=tcp:db;Database=Pwr-fhs-dev;User ID=postgres;Password=example;

