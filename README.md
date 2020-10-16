# :space_invader: Invillia - Borrowing Game API :video_game:

This API is used to manage friends and games which can be borrowed by them. Making it easier to get track of each game that is borrowed.

## Instalation

Use [docker-compose](https://docs.docker.com/compose/install/) to run all the required containers.

```
docker-compose up -d
```

*Obs: If you are using Docker in MacOS add the following paths to the File Sharing at docker:

```
/Microsoft/UserSecrets
/ASP.NET/Https
```

The command should be runned at root folder of the application, where <b>docker-compose.yml</b> is located.


## Usage

After all the containers are up, navigate to https://localhost:5101 to go the swaggerUI page.

The API uses JWT for authorization and has two Roles (User Types):  

  1 - Administrator  
  2 - Friend  

The application is based on the following principles:  

- Administrators can manage games and friends.
- Friends can borrow and return games.
- Each user type can see all the games registered, also their availability.
- Only Friends can borrow and return games;
- Only Administrators can create, edit and delete games;
- Friends can edit their own information.
- Administrators can edit Friends informations.

## Tools

The Application is built on:
- .Net Core 3.1
- Sql Server 2017
- Docker
- Docker-Compose
- Cryptograpy (BCrypt.Net)
- JWT Token
- AutoMapper
- EF Core
- NUnit
