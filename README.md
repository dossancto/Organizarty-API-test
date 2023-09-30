# Organizarty Web API

## Run

- Clone repo
    ```sh
    git clone https://github.com/lu-css/Organizarty-API.git && cd Organizarty-API
    git submodule init
    git submodule update
    ```

- Start database on Docker
    ```sh
    docker compose up -d 
    ```

- Run migrations
    ```sh
    dotnet ef database update -s Organizarty.Application
    ```

- Start server
    ```sh
    dotnet run -p Organizarty.Application
    ```

## On Docker
- Run Docker file

## Development

- Add migrations (In root directory)

    ```sh
    dotnet ef migrations add {migration_name} -s Organizarty.Application -p Organizarty.Infra
    ```

