# ServerAPI
## Описание - суть, идея, замысел
### Уровень 1:
Разработано два сервиса: "Приложение пользователя" и "База данных". Доступ ко второму сервису происходит через первый. "Приложение пользователей" позволяет создатавать пользователей и хранить их покупки. (Для запуска 1 уровня требуется скачать проект с коммита, который называется "first level task completed").

### Уровень 2:
Разработано три сервиса: "Приложение магазинов", "Приложение пользователей" и "База данных".
К приложению "База данных" обращение происходит через оставшийся сервисы, доступ к которым открыт через порты localhost.
Разработанная система позволяет пользователям просматривать магазины, их товары, совершать покупку(с выдачей чека) и просматривать совершённые покупки.
Владельцы магазина могут добавлять товары в свои магазины, изменять их и получать чеки обо всех покупка, совершённых в их магазинах.
Владелец системы может добавлять магазины или удалять их.

Использумеые технологии:
 - Бэкэнд - [ASP.NET Core](https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core)
 - База данных - [MySQL](https://www.mysql.com/)
 - Окружение - [Docker](https://www.docker.com/)
 - Документация API - [Swagger](https://swagger.io/)

## Инструкция по запуску
Необходимо:
 - [Docker Desktop](https://www.docker.com/products/docker-desktop)
 - [Visual Studio](https://visualstudio.microsoft.com/ru/downloads/)(для просмотра исходного кода)

Запускать можно на Windows OS.

Для поднятия сервисов нужно скачать проект и зайти через cmd в папку проекта, где находится docker-compose файл.
В консоли ввести следующие команды:
```sh
docker-compose pull
docker-compose up -d
```
После этого доступ к "Приложению пользователей" можно получить по ссылке: [localhost:8080](http://localhost:8080) (для 1 уровня и для 2)

Доступ к "Приложению магазинов" можно поулчить по ссылке: [localhost:8081](http://localhost:8081)

Докуменатцию по путям в приложениях можно найти по ссылкам:
 - [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)(для 1 уровня и для 2)
 - [http://localhost:8081/swagger/index.html](http://localhost:8081/swagger/index.html)

## Ссылки
 - [Видео с опиcанием проекта 2 уровня](https://disk.yandex.ru/d/keOlHy7JvGfnaA?w=1)
 - [DockerHub для уровня 1](https://hub.docker.com/repository/docker/maxide201/rtu_it_lab_lvl1)
 - [DockerHub для уровня 2](https://hub.docker.com/repository/docker/maxide201/rtu_it_lab_lvl2)
 - [Postman тесты для уровня 1](https://www.getpostman.com/collections/0d60db30e581534d3f40)
 - [Postman тесты для уровня 2](https://www.getpostman.com/collections/c7d0d9778d2c170294db)
