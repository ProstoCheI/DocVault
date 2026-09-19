# Docker в DocVault

## Назначение

Docker обеспечивает подготовку Windows-приложения в описанной среде сборки. Пользовательское приложение сохраняет прежние формы и операции с документами. Графический интерфейс запускается в Windows вне контейнера.

## Изменения относительно feature/docker-integration

- Удалён консольный режим с инициализацией базы и бесконечным heartbeat: он не реализовывал работу пользователя с архивом.
- Восстановлен единый целевой framework `net9.0-windows` и включён `EnableWindowsTargeting`, разрешающий сборку Windows-проекта в Linux SDK.
- Dockerfile публикует автономный `win-x64` EXE. Этап `export` копирует результат для Compose; этап `artifacts` позволяет выгрузить результат через BuildKit.
- Compose содержит одноразовую задачу `build-windows`, без серверного процесса и томов данных приложения.
- Зафиксированы SDK, digest образа и NuGet lock-файл.
- База SQLite привязана к каталогу EXE, как и папка Storage. Исправление предотвращает выбор другого каталога при смене рабочей папки.
- `.dockerignore` исключает архивные файлы, базы, результаты сборки и локальные настройки из контекста.

## Исправленная проблема томов

В исходной ветке том `docvault_db` монтировался в `/app/data`, тогда как БД создавалась по пути `/app/docvault.db`. Такая конфигурация не сохраняла БД при пересоздании контейнера. В выбранной архитектуре приложение вообще не хранит рабочие данные в контейнере: они находятся рядом с EXE в Windows. Контейнер отвечает только за сборку.

## Запуск сборки

```powershell
docker compose run --build --rm build-windows
```

Результат — `artifacts/DocVaultLocal.exe`. Перед обычной работой перенесите его в отдельную папку Windows. Сборка и пользовательский архив должны быть разделены.

## Проверки

Публикация автономного EXE, проверка запуска и инициализации каталога, тесты CRUD и стабильного пути базы, проверка конфигурации Compose выполнены успешно. Linux-сборка в Docker ещё требует проверки на работающем Docker Engine: в использованной среде Docker Desktop не запустился.

## Источники

- Microsoft: EnableWindowsTargeting — https://learn.microsoft.com/en-us/dotnet/core/tools/sdk-errors/netsdk1100
- Microsoft: ограничения интерактивных приложений в Windows containers — https://learn.microsoft.com/en-us/virtualization/windowscontainers/quick-start/lift-shift-to-containers
- Docker: local exporter — https://docs.docker.com/build/exporters/local-tar/
