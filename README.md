# 🛰️ SatelliteTracker

**SatelliteTracker** — это серверное приложение на ASP.NET Core, предназначенное для приёма, обработки и трансляции GPS/NMEA-данных со спутниковых приёмников в реальном времени через WebSocket.

> ⚠️ **Примечание:** на текущем этапе проект работает **без подключения к базе данных** — используется мок-репозиторий для имитации работы с данными.

---

## 📁 Структура проекта

```
SatelliteTracker.Backend/
│
├── Middleware/
│   └── WebSocketMiddleware.cs           # Обработка WebSocket-соединений
│
├── Models/
│   ├── AppDbContext.cs                  # DbContext для EF Core
│   └── SatelliteData.cs                 # Модель данных о спутнике
│
├── Repositories/
│   ├── Interfaces/
│   │   └── ISatelliteDataRepository.cs  # Интерфейс доступа к данным
│   ├── SatelliteDataRepository.cs       # Репозиторий на EF Core
│   └── MockSatelliteDataRepository.cs   # Мок-репозиторий (данные в памяти)
│
├── Services/
│   ├── Gps/
│   │   ├── GpsDataBackgroundService.cs  # Фоновый сервис чтения GPS-данных
│   │   ├── GpsSettings.cs               # Модель конфигурации GPS
│   │   ├── IGpsReader.cs                # Интерфейс для чтения GPS
│   │   ├── SerialGpsReader.cs           # Чтение данных с COM-порта
│   │   └── SimulatedGpsReader.cs        # Генерация симулированных GPS-данных
│   │
│   ├── Interfaces/
│   │   └── INmeaParserService.cs        # Интерфейс сервиса разбора NMEA
│   ├── NmeaParserService.cs             # Разбор NMEA-сообщений
│   └── WebSocketConnectionManager.cs    # Менеджер WebSocket-подключений
│
├── wwwroot/
│   └── index.html                       
│
├── appsettings.json                     
├── Program.cs    
├── SatelliteTracker.Backend.csproj      # Файл проекта
├── README.md                            # Этот файл :)
```

---

## ⚙️ Возможности

- Приём GPS-данных с COM-порта (включая симуляцию)
- Разбор NMEA-сообщений
- Отправка актуальных данных клиентам через WebSocket
- Регистрация и хранение данных в мок-репозитории (в памяти)
- HealthCheck-эндпоинт: `GET /health`

---
## ⚙️ Как запустить (симуляция)

1. Убедитесь, что в `appsettings.json` включён режим симуляции:
   ```json
   "GpsSettings": {
     ...
     "SimulationMode": true
   }
   ```

2. Убедитесь, что в `Program.cs` зарегистрирован `MockSatelliteDataRepository`:
   ```csharp
   builder.Services.AddSingleton<ISatelliteDataRepository, MockSatelliteDataRepository>();
   ```

3. Запуск:
   ```bash
   dotnet build
   dotnet run
   ```

4. WebSocket-соединение доступно по адресу:  
   ```
   ws://localhost:5008/ws
   ```

---

## 💬 Пример WebSocket-данных

```json
{
  "id": 0,
  "system": "GPS",
  "time": "2025-04-21T17:30:00",
  "elevation": 35,
  "azimuth": 120,
  "snr": 45
}
```

---

## 🛠️ В планах

- Подключение PostgreSQL и полноценного репозитория
- Расширение парсера NMEA под другие типы сообщений и систем (GLONASS, Galileo)
- Реализация REST API
- Веб-интерфейс на фронтенде для отображения спутников на карте

---

## 📚 Технологии

- .NET 9 / ASP.NET Core
- WebSocket
- NMEA-парсинг
- Background services
- PostgreSQL (в будущем)
