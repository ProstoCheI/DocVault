# Отчет по интеграции Docker в проект DocVault

## 1. Введение и цель работы
Цель работы — контейнеризация проекта **DocVault** с использованием технологии **Docker**, обеспечивающей изоляцию окружения, переносимость и воспроизводимость сборки на базе платформы **.NET 9**.

---

## 2. Архитектура решения и ветвление в Git

Для изоляции изменений от основной кодовой базы была создана отдельная ветка:
```bash
git checkout -b feature/docker-integration
```

### Мультитаргетинг (Cross-Platform .NET 9)
Исходный проект ориентирован на Windows Forms (`net9.0-windows`). Для поддержки работы в среде Linux-контейнеров без нарушения десктопной версии был настроен мультитаргетинг в файле `DocVaultLocal.csproj`:
* При компиляции на Windows-хосте собирается `net9.0-windows` (GUI интерфейс).
* При сборке внутри Linux-контейнера Docker компилируется `net9.0` (фоновая служба DocVault Service, выполняющая инициализацию базы данных SQLite и управление файловым хранилищем).

---

## 3. Конфигурация Dockerfile (Multi-Stage Build)

Был спроектирован многоэтапный `Dockerfile` для оптимизации размера конечного образа:

```dockerfile
# Этап 1: Сборка и компиляция приложения в образе SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копирование файла проекта и восстановление зависимостей
COPY ["DocVaultLocal.csproj", "./"]
RUN dotnet restore "DocVaultLocal.csproj" /p:TargetFramework=net9.0

# Копирование исходного кода и публикация Release-сборки
COPY . .
RUN dotnet publish "DocVaultLocal.csproj" -c Release -f net9.0 -o /app/publish

# Этап 2: Финальный образ для запуска (минимальный runtime)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app

# Создание директории для хранения документов
RUN mkdir -p /app/Storage

# Копирование скомпилированных файлов из этапа сборки
COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=Production

# Точка входа контейнера
ENTRYPOINT ["dotnet", "DocVaultLocal.dll"]
```

---

## 4. Конфигурация Docker Compose

Файл `docker-compose.yml` описывает сервис и постоянные тома (Volumes) для сохранения данных:

```yaml
services:
  docvault-app:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: docvault-container
    restart: unless-stopped
    volumes:
      - docvault_storage:/app/Storage
      - docvault_db:/app/data

volumes:
  docvault_storage:
  docvault_db:
```

---

## 5. Основные команды для сборки и демонстрации

1. **Сборка Docker-образа:**
   ```bash
   docker compose build
   ```
2. **Запуск контейнера в фоновом режиме:**
   ```bash
   docker compose up -d
   ```
3. **Просмотр логов работы контейнера:**
   ```bash
   docker logs docvault-container
   ```
4. **Проверка статуса активных контейнеров:**
   ```bash
   docker ps
   ```
5. **Остановка сервиса:**
   ```bash
   docker compose down
   ```
