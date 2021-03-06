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

### Running in a Docker container - Ubuntu 16.04 LTS

Place e-mail account configuration (and possibly database configuration) in
`appsettings.json`. An example file looks like this:

    {
      "ConnectionStrings": {
        "DatabaseConnection": "Server=db;Database=Pwr-fhs-dev;User ID=postgres;Password=example;"
      },
      "ImapServer": "",
      "ImapPort": 993,
      "SmtpServer": "",
      "SmtpPort": 587,
      "MailAddress": "",
      "MailLogin": "",
      "MailPassword": "",
      "DoMailboxFetching": true,
      "CheckInterval": 15,
	  "AdminContact": {
		"ContactLine1": "FirstName LastName",
		"ContactLine2": "admin@example.com",
		"ContactLine3": "+48 999 888 777" 
	  },
	  "Logging": {
		"IncludeScopes": false,
		"LogLevel": {
			"Default": "Warning"
		}
	  }
    }

Fill in the correct values.

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
