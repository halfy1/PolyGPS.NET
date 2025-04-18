
# 🛰️ SatelliteTracker

**SatelliteTracker** — это серверное приложение на ASP.NET Core, предназначенное для приёма, обработки и трансляции GPS/NMEA-данных со спутниковых приёмников в реальном времени через WebSocket.

> ⚠️ **Примечание:** на текущем этапе проект работает **без подключения к базе данных** — используется мок-репозиторий для имитации работы с данными.

---

## 📁 Структура проекта

```
SatelliteTracker.Backend/
│
├── Controllers/                  
│   └── (в разработке)            # Здесь будут API-контроллеры
│
├── Middleware/
│   └── WebSocketMiddleware.cs    # Обработка WebSocket-соединений, маршрутизация сообщений
│
├── Models/
│   └── SatelliteData.cs          # Модель данных о спутнике (время, система, ID, высота, скорость и др.)
│
├── Repositories/
│   ├── Interfaces/
│   │   └── ISatelliteDataRepository.cs   # Интерфейс для доступа к данным
│   ├── SatelliteDataRepository.cs        # Репозиторий на EF Core (не используется в mock-режиме)
│   └── MockSatelliteDataRepository.cs    # Мок-репозиторий, сохраняет данные в памяти
│
├── Services/
│   ├── Interfaces/
│   │   └── INmeaParserService.cs         # Интерфейс сервиса разбора NMEA-сообщений
│   ├── NmeaParserService.cs              # Простой разбор GPS-сообщений формата NMEA
│   └── GpsDataBackgroundService.cs       # Фоновый сервис чтения GPS-данных с COM-порта или в симуляции
│
├── WebSocketConnectionManager.cs         # Менеджер активных WebSocket-соединений
├── AppDbContext.cs                       # DbContext (не используется в mock-режиме)
├── Program.cs                            # Точка входа, конфигурация приложения
├── appsettings.json                      # Настройки приложения (COM-порт, WebSocket, логирование и др.)
```

---

## ⚙️ Возможности

- Приём GPS-данных с COM-порта (включая симуляцию)
- Разбор NMEA-сообщений
- Отправка актуальных данных клиентам через WebSocket
- Регистрация и хранение данных в мок-репозитории (в памяти)
- HealthCheck-эндпоинт: `GET /health`

---

## 🧪 Запуск в mock-режиме

1. Убедитесь, что в `Program.cs` зарегистрирован `MockSatelliteDataRepository`:
   ```csharp
   builder.Services.AddSingleton<ISatelliteDataRepository, MockSatelliteDataRepository>();
   ```

2. Убедитесь, что в `appsettings.json` установлено:
   ```json
   "GpsSettings": {
     ...
     "SimulationMode": true
   }
   ```

3. Соберите и запустите проект:
   ```bash
   dotnet build
   dotnet run
   ```

Вы увидите логи вида:
```
[MockRepo] Данные спутника добавлены: ID=4, System=GPS, Time=18.04.2025 11:04:25, Elevation=40°, Azimuth=230°, SNR=46 dB
```

---

## 🗂️ Планируемые улучшения

- Подключение PostgreSQL через Entity Framework Core
- REST API-контроллеры для запроса истории спутниковых данных
- Веб-интерфейс для отображения текущего положения спутников
- Поддержка нескольких форматов (GLONASS, Galileo)

---

## 📋 Зависимости

- .NET 8 / ASP.NET Core
- Entity Framework Core
- PostgreSQL (будет использоваться позже)
- WebSocket

---