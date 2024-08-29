using Domain.Entities;

namespace UnitTests.Domain.Entities
{
    public class RentalClosureEntityTest
    {
        private readonly PlanEntity _planSevenDays;
        private readonly PlanEntity _planFifteenDays;

        public RentalClosureEntityTest()
        {
            _planSevenDays = new PlanEntity(7, 30.0m, 20.0m);
            _planFifteenDays = new PlanEntity(15, 28.0m, 40.0m);
        }

        [Fact]
        public void ShouldCalculatedWhenNoDailyHaveBeenUsedInTheSevenDayPlan()
        {
            var startDate = new DateTime(2024, 8, 17);
            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planSevenDays, startDate, startDate.AddDays(7));
            rentEntity.CloseRental(startDate);

            Assert.Equal(0, rentEntity.RentalClosure.CostForUsedDays);
            Assert.Equal(252, rentEntity.RentalClosure.PenaltyAmountForUnusedDay);
            Assert.Equal(252, rentEntity.RentalClosure.TotalPayment);
        }

        [Fact]
        public void ShouldCalculatedWhenNoDailyHaveBeenUsedInTheSevenDayPlanAndTwoExcededDailies()
        {
            var startDate = new DateTime(2024, 8, 17);
            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planSevenDays, startDate, startDate.AddDays(9));
            rentEntity.CloseRental(startDate.AddDays(9));

            Assert.Equal(210, rentEntity.RentalClosure.CostForUsedDays);
            Assert.Equal(0, rentEntity.RentalClosure.PenaltyAmountForUnusedDay);
            Assert.Equal(310, rentEntity.RentalClosure.TotalPayment);
        }

        [Fact]
        public void ShouldCalculatedWhenAllDailyHaveBeenUsedInTheSevenDayPlan()
        {
            var startDate = new DateTime(2024, 8, 17);
            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planSevenDays, startDate, startDate.AddDays(7));

            Assert.Equal(210, rentEntity.RentalClosure.CostForUsedDays);
            Assert.Equal(0, rentEntity.RentalClosure.PenaltyAmountForUnusedDay);
            Assert.Equal(210, rentEntity.RentalClosure.TotalPayment);
        }

        [Fact]
        public void ShouldCalculatedWhenTwoDailyRatesAreNotUsedInTheSevenDayPlan()
        {
            var startDate = new DateTime(2024, 8, 17);
            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planSevenDays, startDate, startDate.AddDays(5));

            Assert.Equal(150, rentEntity.RentalClosure.CostForUsedDays);
            Assert.Equal(72, rentEntity.RentalClosure.PenaltyAmountForUnusedDay);
            Assert.Equal(222, rentEntity.RentalClosure.TotalPayment);
        }

        [Fact]
        public void ShouldCalculatedWhenTwoDailyRatesAreNotUsedInTheFifteenDaysPlan()
        {
            var startDate = new DateTime(2024, 8, 17);
            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planFifteenDays, startDate, startDate.AddDays(13));

            Assert.Equal(364, rentEntity.RentalClosure.CostForUsedDays);
            Assert.Equal(78.4m, rentEntity.RentalClosure.PenaltyAmountForUnusedDay);
            Assert.Equal(442.4m, rentEntity.RentalClosure.TotalPayment);
        }

        [Fact]
        public void ShouldCalculatedWhenExpectedEndDateIsGreaterThanTheExpiryDate()
        {
            var startDate = new DateTime(2024, 8, 17);
            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planFifteenDays, startDate, startDate.AddDays(17));

            Assert.Equal(420, rentEntity.RentalClosure.CostForUsedDays);
            Assert.Equal(0, rentEntity.RentalClosure.PenaltyAmountForUnusedDay);
            Assert.Equal(520, rentEntity.RentalClosure.TotalPayment);
        }
    }
}
