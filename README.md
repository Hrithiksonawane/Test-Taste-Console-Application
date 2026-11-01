# Test & Taste - Solar System Console Application

A .NET 8.0 console application that fetches Solar System data from the [Solar System OpenData API](https://api.le-systeme-solaire.net) and displays planet and moon information.

## Features

- **Average Moon Temperature** - Lists all planets with at least one moon and the average temperature of their moons
- **Average Moon Gravity** - Displays the average moon gravity for each planet
- **Moon Mass Data** - Lists all moons with their mass values and exponents
- **Planets & Moons** - Displays each planet with its semi-major axis and individual moons
- **CSV File Export** - Writes all output data to CSV files in the `FileOutput` folder
- **Error Handling** - Gracefully handles API failures without crashing

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- API Key from [Solar System OpenData](https://api.le-systeme-solaire.net/en/generatekey.html)

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/Hrithiksonawane/Test-Taste-Console-Application.git
   cd Test-Taste-Console-Application
   ```

2. Navigate to the project folder:
   ```bash
   cd Test-Taste-Console-Application
   ```

3. Copy the example config and add your API key:
   ```bash
   cp appsettings.example.json appsettings.json
   ```
   Then open `appsettings.json` and replace `YOUR_API_KEY_HERE` with your actual API key.

4. Run the application:
   ```bash
   dotnet run
   ```

## Project Structure

```
Test-Taste-Console-Application/
├── Constants/
│   ├── OutputString.cs          # Console output labels
│   ├── PathName.cs              # File output paths
│   └── UriPath.cs               # API endpoint paths
├── Domain/
│   ├── DataTransferObjects/
│   │   ├── MoonDto.cs           # Moon API response DTO
│   │   └── PlanetDto.cs         # Planet API response DTO
│   ├── Objects/
│   │   ├── Moon.cs              # Moon domain model
│   │   └── Planet.cs            # Planet domain model
│   └── Services/
│       ├── Interfaces/
│       │   ├── IMoonService.cs
│       │   ├── IOutputService.cs
│       │   └── IPlanetService.cs
│       ├── FileOutputService.cs  # CSV file export
│       ├── HttpClientService.cs  # API client with Bearer auth
│       ├── MoonService.cs        # Moon data fetching
│       ├── PlanetService.cs      # Planet data fetching
│       └── ScreenOutputService.cs # Console output
├── appsettings.example.json      # Config template
└── Program.cs                    # Entry point and DI setup
```

## Output

The application produces:

**Console Output** - Formatted tables for each data section with loading/completed status messages.

**CSV Files** (in `FileOutput/`):
- `AllPlanetsAndTheirAverageMoonTemperature.csv`
- `AllMoonsAndTheirMass.csv`
- `AllPlanetsAndTheirMoons.csv`

## Notes

- Moon temperatures display as 0.00 K because the API does not provide temperature data for moons. The feature is implemented correctly; the data simply does not exist in the dataset.
- Planet names are displayed in French as returned by the API (e.g., Terre = Earth, Saturne = Saturn).
