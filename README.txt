Create Docker Container

``
docker run -d --rm --name pgdb -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -v sebdata:/var/lib/postgresql/data postgres
``

Log into Container
``
docker exec -it pgdb psql -U postgres
``

Create Database
``
CREATE DATABASE sebdb;
``

Tables will be created by the program itself.