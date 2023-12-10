using Task_02.Persistence.Entities;

namespace Task_02.Services.Interfaces;

public interface ITurnoverExcelParser
{
    public Task<TurnoverStatement> ParseAsync(string fileName);
}