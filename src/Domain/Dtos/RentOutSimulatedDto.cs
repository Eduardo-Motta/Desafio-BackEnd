namespace Domain.Dtos
{
    public record RentOutSimulatedDto(int ExceededDays
        , decimal TotalPayment
        , decimal CostForUsedDays
        , decimal PenaltyAmountForUnusedDay
        , decimal ValueOfAdditionalDailyRates)
    { }
}
