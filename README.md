# Docker Sql Server Integration

The purpose of this repository is to demonstrate how to setup and use Sql Server running in a docker container during development. 

### Getting Started

1. Download and install [Docker](https://www.docker.com/get-started/)
2. Clone the repository
3. Navigate to the `Docker` folder containing the  `docker-compose.yml` file. 
4. Open a terminal and run the following command:
   
   ```bash
   docker-compose up -d
   ```
   This spins up a sql server container in a detached mode.

5. If using Visual Studio, open up the package manager console and run the following command:

```bash
update-database
```
This will run EF Cores migrations, creating the database in the running Sql Server container.

If you prefer to run the migrations from the command line instead, run the following command:

```bash
dotnet ef database update
```

You should now have a working application that will allow you to develop your application using a full Sql Server instance without the need for installing Sql Server directly on your machine. 

There are several things to note about the `docker-compose.yml` file.

```bash
version: '3.7'
services:
  sql-server-db:
    container_name: sql-server-db
    image: mcr.microsoft.com/mssql/server:2019-latest 
    ports:
      - "1433:1433" // the port you want to connect to your sql server instance. 1433 is the default port. If you want to change the port, you will update the first number, i.e. 5033:1433 will run the container on localhost:5033 
    environment:
      SA_PASSWORD: "P@55word123"
      ACCEPT_EULA: "Y"
    volumes: // the location where to persist your data. Currently this will create the folders in the same directory as the docker-compose file. If you want, you can update this.
      - "./data/:/var/opt/mssql/data"
      - "./log/:/var/opt/mssql/log"
      
```

For more information on using Docker Compose, see this [link](https://www.docker.com/blog/tag/docker-compose/).

To shut down the running Sql Server container run the following command:

```bash
docker-compose down
```

All your data will be persisted in the location listed in the `docker-compose.yml` file. When you spin the container back up, all the changes you made during previous sessions will still be available. 

### Connecting to your database using SSMS

1. Open SSMS
2. Fill out the following:
  Server type: Database Engine
  Server name: localhost, 1433
  Authentication: SQL Server Authentication
  Login: sa
  Password: P@55word123

You should now be connected to your database.