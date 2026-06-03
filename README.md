# MusicTracker

A backend service built with **ASP.NET Core 9** that pulls your listening history from [Last.fm](https://www.last.fm) and exposes clean analytics endpoints — top tracks, top artists, and listening stats for any time period.

---

## Features

- **Sync** — fetches a user's full scrobble history from Last.fm and stores it locally, including track duration via `track.getInfo`
- **Incremental updates** — re-syncing only pulls plays newer than the last stored entry, so you never get duplicates
- **Top tracks** — returns the 10 most-played tracks for a user
- **Top artists** — returns the 5 most-played artists for a user
- **Period statistics** — total plays, unique artists, and total listening time for any date range

---

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 9 |
| Framework | ASP.NET Core 9 |
| ORM | Entity Framework Core 9 |
| Database | PostgreSQL (via Npgsql) |
| External API | Last.fm REST API |

The solution follows a clean **layered architecture**:

```
MusicTracker.sln
├── API              — controllers, middleware, DI wiring
├── Application      — services, interfaces, DTOs
├── Domain           — entities (Track, ListeningHistory)
└── Infrastructure   — EF Core, repositories, Last.fm HTTP client
```

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- PostgreSQL (local or Docker)
- A free [Last.fm API key](https://www.last.fm/api/account/create)

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/your-username/last-fm-api.git
cd last-fm-api/MusicTracker
```

### 2. Configure the application

Open `API/appsettings.json` (or use `appsettings.Development.json` / environment variables) and fill in your values:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=musicdb;Username=postgres;Password=your_password"
  },
  "MusicApi": {
    "ApiKey": "your_lastfm_api_key"
  }
}
```

Alternatively, copy `.env.example` to `.env` and set the values there — the project reads `CONNECTION_STRING` and uses the `MusicApi:ApiKey` config key.

### 3. Apply database migrations

```bash
cd Infrastructure
dotnet ef database update --startup-project ../API
```

This runs all three migrations and creates the `Tracks` and `ListeningHistories` tables automatically.

### 4. Run the API

```bash
cd ../API
dotnet run
```

The API starts on `https://localhost:5001` (or `http://localhost:5000`). Swagger UI is available at `/swagger`.

---

## API Reference

### Sync

#### `POST /sync/{username}`

Fetches the user's recent scrobbles from Last.fm and saves them to the database. Safe to call multiple times — only new plays are added.

```http
POST /sync/rj
```

**Response:** `200 OK` (empty body on success)

**Error responses:**

| Status | Reason |
|---|---|
| `404 Not Found` | Last.fm user does not exist |
| `502 Bad Gateway` | Last.fm API is unavailable |

---

### Tracks

#### `GET /tracks/top/tracks?username={username}`

Returns the top 10 most-played tracks for the user.

```http
GET /tracks/top/tracks?username=rj
```

**Response:**

```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Bohemian Rhapsody",
    "duration": "00:05:55",
    "artistName": "Queen"
  }
]
```

---

#### `GET /tracks/top/artists?username={username}`

Returns the top 5 most-played artists for the user.

```http
GET /tracks/top/artists?username=rj
```

**Response:**

```json
[
  "Queen",
  "The Beatles",
  "Pink Floyd",
  "Led Zeppelin",
  "David Bowie"
]
```

---

#### `GET /tracks/statistic?username={username}&from={date}&to={date}`

Returns listening statistics for the given date range.

```http
GET /tracks/statistic?username=rj&from=2024-01-01&to=2024-12-31
```

| Parameter | Format | Description |
|---|---|---|
| `username` | string | Last.fm username |
| `from` | `YYYY-MM-DD` | Start of the period (inclusive) |
| `to` | `YYYY-MM-DD` | End of the period (exclusive) |

**Response:**

```json
{
  "trackCount": 1042,
  "artistCount": 87,
  "generalDuration": "2.14:32:10"
}
```

`generalDuration` is returned as a .NET `TimeSpan` string (`d.HH:mm:ss`).

**Error responses:**

| Status | Reason |
|---|---|
| `400 Bad Request` | `from` date is not earlier than `to` date |
| `404 Not Found` | No listening history found for this user |

---

## Database Schema

```
Tracks
  Id          UUID  PK
  Name        TEXT  NOT NULL
  ArtistName  TEXT  NOT NULL
  Duration    INTERVAL
  UNIQUE (Name, ArtistName)

ListeningHistories
  Id        UUID      PK
  TrackId   UUID      FK → Tracks.Id
  UserName  TEXT      NOT NULL
  PlayedAt  TIMESTAMP NOT NULL
```

Migrations are managed with EF Core and live in `Infrastructure/Migrations/`.

---

## Postman Collection

A ready-to-use Postman collection with example requests for all endpoints is included in the repository root as `MusicTracker.postman_collection.json`.

Import it via **File → Import** in Postman and set the `baseUrl` variable to match your running instance (default: `http://localhost:5000`).

---

## How Sync Works

1. Calls `user.getrecenttracks` on the Last.fm API to get the user's full scrobble history (currently-playing tracks are excluded).
2. For each track not yet in the local database, calls `track.getInfo` to fetch the duration and stores the track.
3. Builds `ListeningHistory` records and filters out any plays already stored (based on the last recorded `PlayedAt` timestamp for that user).
4. Persists everything in a single batch.

---

## Notes

- The Last.fm free API returns up to **200 recent tracks** per request. If you need full history pagination, that can be added to `MusicApiClient`.
- Top tracks and top artists limits (10 and 5 respectively) are constants in `TrackService` — easy to change or expose as query parameters.
- All unhandled exceptions are caught by `ExceptionMiddleware`, which maps domain exceptions (`NotFoundException`, `BadRequestException`, `ExternalServiceException`) to appropriate HTTP status codes.
