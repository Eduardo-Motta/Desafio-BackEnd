namespace Domain.Dtos
{
    public record RentOutDto(Guid Id
        , Guid PlanId
        , Guid MotorcycleId
        , DateTime StartDate
        , DateTime ExpectedEndDate
        , decimal ExpectedTotalRentalAmount)
    { }
}
