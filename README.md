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
  1. Use the following commands to build and run your Docker image:
```
    $ docker build -t fault-handling-system .
    $ docker run -p 10000:80 fault-handling-system
```

Now you can navigate your browser to http://localhost:10000/
