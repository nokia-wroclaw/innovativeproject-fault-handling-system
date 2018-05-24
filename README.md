# Fault Handling System

## Administrator Guide

### Preparing Docker environment - Ubuntu 16.04 LTS

First, you have to have Docker installed on your system.

As a post-installation step, add yourself to `docker` group:

`sudo usermod -aG docker <username>`

and log out and log in again for the change to take effect.

### Build and deployment

Open a command prompt and navigate to the project folder
`Fault-handling-system/`. Then use the following commands to build
your Docker image:
```
    $ docker-compose build
```

Then you have to apply migrations to the database. To do this:

1. Comment out `fault-handling-system` container in `docker-compose.yml`
to prevent it from starting.

2. Launch the database container:
```
docker-compose up
```

3. Export the variable pointing to the database inside the container:
```
export ConnectionStrings__DefaultConnection="Server=localhost;Port=10001;Database=Pwr-fhs-dev;User ID=postgres;Password=example;"
```

4. Perform the migrations on the selected database:
```
dotnet ef database update
```

### Running in a Docker container - Ubuntu 16.04 LTS

First, prepare environment files for the database and the app: `db.env`
and `fhs.env`. Place e-mail account configuration (and possibly database
configuration) in `fhs.env`.

It is recommended to start both PostgreSQL
and the application using `docker-compose`:
```
    $ docker-compose up
```

Now you can navigate your browser to http://localhost:8080/
(or http://localhost:10000/ for PostgreSQL administration).

When first run, the application creates an admin account
with email admin@example.com and password "Admin-123".
You can use it to log in.
