using System;
using System.IO;
using System.Linq;
using System.Text;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Objects;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application.Domain.Services
{
    /// <summary>
    /// Writes planet and moon data to CSV files in the FileOutput folder.
    /// </summary>
    public class FileOutputService
    {
        private readonly IPlanetService _planetService;
        private readonly IMoonService _moonService;

        public FileOutputService(IPlanetService planetService, IMoonService moonService)
        {
            _planetService = planetService;
            _moonService = moonService;
        }

        /// <summary>
        /// Writes all planets (with at least one moon) and their average moon temperature to a CSV file.
        /// </summary>
        public void WriteAllPlanetsAndTheirAverageMoonTemperatureToFile()
        {
            Console.WriteLine("Writing average moon temperature data to file...");

            var planets = _planetService.GetAllPlanets().ToArray();

            if (!planets.Any())
            {
                Console.WriteLine(OutputString.NoPlanetsFound);
                return;
            }

            try
            {
                EnsureOutputFolderExists();

                var filePath = Path.Combine(PathName.PathToOutputFolder, PathName.AllPlanetsAndTheirAverageMoonTemperatureFile);
                var sb = new StringBuilder();
                sb.AppendLine("Planet Id,Average Moon Temperature (K)");

                foreach (Planet planet in planets)
                {
                    if (planet.HasMoons())
                    {
                        sb.AppendLine($"{CultureInfoUtility.TextInfo.ToTitleCase(planet.Id)},{planet.AverageMoonTemperature:F2}");
                    }
                }

                File.WriteAllText(filePath, sb.ToString());
                Console.WriteLine($"{OutputString.FileCreated}{filePath}");
            }
            catch (IOException ex)
            {
                Logger.Instance.Error($"Failed to write temperature file: {ex.Message}");
                Console.WriteLine($"Error: Could not write temperature file. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes all moons and their mass data to a CSV file.
        /// </summary>
        public void WriteAllMoonsAndTheirMassToFile()
        {
            Console.WriteLine("Writing moon mass data to file...");

            var moons = _moonService.GetAllMoons().ToArray();

            if (!moons.Any())
            {
                Console.WriteLine(OutputString.NoMoonsFound);
                return;
            }

            try
            {
                EnsureOutputFolderExists();

                var filePath = Path.Combine(PathName.PathToOutputFolder, PathName.AllMoonsAndTheirMassFile);
                var sb = new StringBuilder();
                sb.AppendLine("Moon Id,Mass Value,Mass Exponent");

                foreach (var moon in moons)
                {
                    sb.AppendLine($"{CultureInfoUtility.TextInfo.ToTitleCase(moon.Id)},{moon.MassValue},{moon.MassExponent}");
                }

                File.WriteAllText(filePath, sb.ToString());
                Console.WriteLine($"{OutputString.FileCreated}{filePath}");
            }
            catch (IOException ex)
            {
                Logger.Instance.Error($"Failed to write moon mass file: {ex.Message}");
                Console.WriteLine($"Error: Could not write moon mass file. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes all planets and their moons to a CSV file.
        /// </summary>
        public void WriteAllPlanetsAndTheirMoonsToFile()
        {
            Console.WriteLine("Writing planet and moon data to file...");

            var planets = _planetService.GetAllPlanets().ToArray();

            if (!planets.Any())
            {
                Console.WriteLine(OutputString.NoPlanetsFound);
                return;
            }

            try
            {
                EnsureOutputFolderExists();

                var filePath = Path.Combine(PathName.PathToOutputFolder, PathName.AllPlanetsAndTheirMoonsFile);
                var sb = new StringBuilder();
                sb.AppendLine("Planet Id,Semi-Major Axis,Moon Id");

                foreach (var planet in planets)
                {
                    if (planet.HasMoons())
                    {
                        foreach (var moon in planet.Moons)
                        {
                            sb.AppendLine($"{CultureInfoUtility.TextInfo.ToTitleCase(planet.Id)},{planet.SemiMajorAxis},{CultureInfoUtility.TextInfo.ToTitleCase(moon.Id)}");
                        }
                    }
                    else
                    {
                        sb.AppendLine($"{CultureInfoUtility.TextInfo.ToTitleCase(planet.Id)},{planet.SemiMajorAxis},");
                    }
                }

                File.WriteAllText(filePath, sb.ToString());
                Console.WriteLine($"{OutputString.FileCreated}{filePath}");
            }
            catch (IOException ex)
            {
                Logger.Instance.Error($"Failed to write planets and moons file: {ex.Message}");
                Console.WriteLine($"Error: Could not write planets and moons file. {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the FileOutput folder if it does not already exist.
        /// </summary>
        private static void EnsureOutputFolderExists()
        {
            if (!Directory.Exists(PathName.PathToOutputFolder))
            {
                Directory.CreateDirectory(PathName.PathToOutputFolder);
                Console.WriteLine($"{OutputString.FolderCreated}{PathName.PathToOutputFolder}");
            }
        }
    }
}
