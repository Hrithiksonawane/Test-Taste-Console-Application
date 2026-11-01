namespace Test_Taste_Console_Application.Domain.Services.Interfaces
{
    ///<summary>
    /// An output service that can show data from the Solar System OpenData API<see href="https://api.le-systeme-solaire.net/"/> to a user via the console. 
    ///</summary>
    public interface IOutputService
    {
        void OutputAllPlanetsAndTheirMoonsToConsole();
        void OutputAllMoonsAndTheirMassToConsole();
        void OutputAllPlanetsAndTheirAverageMoonGravityToConsole();

        /// <summary>
        /// Outputs a table of all planets (that have at least one moon) with the average temperature of their moons in Kelvin.
        /// </summary>
        void OutputAllPlanetsAndTheirAverageMoonTemperatureToConsole();
    }
}
