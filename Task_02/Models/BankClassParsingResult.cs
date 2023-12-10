using Task_02.Persistence.Entities;

namespace Task_02.Models;

public record BankClassParsingResult(
    byte ClassNumber,
    string ClassName,
    List<AccountTurnoverStatement> AccountTurnoverStatements);