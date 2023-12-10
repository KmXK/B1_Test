using Task_02.Persistence.Entities;

namespace Task_02.Services.Interfaces;

public interface ITurnoverExcelImporter
{
    /// <summary>
    /// Imports turnover statement from excel to database.
    /// </summary>
    /// <param name="fileName">Full file name for excel file.</param>
    /// <returns>Imported turnover statement.</returns>
    public Task<TurnoverStatement> ImportAsync(string fileName);
}