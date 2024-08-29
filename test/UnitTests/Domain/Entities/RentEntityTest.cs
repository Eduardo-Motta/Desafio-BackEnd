using Domain.Entities;
using Domain.Enums;

namespace UnitTests.Domain.Entities
{
    public class RentEntityTest
    {
        private readonly PlanEntity _planSevenDays;
        private readonly PlanEntity _planFifteenDays;
        private readonly PlanEntity _planThirtyDays;
        private readonly PlanEntity _planFortyFiveDays;
        private readonly PlanEntity _planFiftyDays;

        public RentEntityTest()
        {
            _planSevenDays = new PlanEntity(7, 30.0m, 20.0m);
            _planFifteenDays = new PlanEntity(15, 28.0m, 40.0m);
            _planThirtyDays = new PlanEntity(30, 22.0m, 0);
            _planFortyFiveDays = new PlanEntity(45, 20.0m, 0);
            _planFiftyDays = new PlanEntity(50, 18.0m, 0);
        }

        [Fact]
        public void ShouldCalculateWhenPlanIsSevenDays()
        {
            var startDate = new DateTime(2024, 8, 17, 0, 0, 0, DateTimeKind.Utc);
            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planSevenDays, startDate, new DateTime(2024, 8, 24, 0, 0, 0, DateTimeKind.Utc));

            Assert.Equal(new DateTime(2024, 8, 18, 0, 0, 0, DateTimeKind.Utc), rentEntity.StartDate);
            Assert.Equal(new DateTime(2024, 8, 24, 0, 0, 0, DateTimeKind.Utc), rentEntity.ExpectedEndDate);
            Assert.Equal(210, rentEntity.TotalPayment);
        }

        [Fact]
        public void ShouldCalculateWhenPlanIsFifteenDays()
        {
            var startDate = new DateTime(2024, 8, 17, 0, 0, 0, DateTimeKind.Utc);

            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planFifteenDays, startDate, new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.Equal(new DateTime(2024, 8, 18, 0, 0, 0, DateTimeKind.Utc), rentEntity.StartDate);
            Assert.Equal(new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Utc), rentEntity.ExpectedEndDate);
            Assert.Equal(420, rentEntity.TotalPayment);
        }

        [Fact]
        public void ShouldCalculateWhenPlanIsThirtyDays()
        {
            var startDate = new DateTime(2024, 8, 17, 0, 0, 0, DateTimeKind.Utc);

            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planThirtyDays, startDate, new DateTime(2024, 9, 16, 0, 0, 0, DateTimeKind.Utc));

            Assert.Equal(new DateTime(2024, 8, 18, 0, 0, 0, DateTimeKind.Utc), rentEntity.StartDate);
            Assert.Equal(new DateTime(2024, 9, 16, 0, 0, 0, DateTimeKind.Utc), rentEntity.ExpectedEndDate);
            Assert.Equal(660, rentEntity.TotalPayment);
        }

        [Fact]
        public void ShouldCalculateWhenPlanIsFiveDays()
        {
            var startDate = new DateTime(2024, 8, 17, 0, 0, 0, DateTimeKind.Utc);

            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planFortyFiveDays, startDate, new DateTime(2024, 10, 1));

            Assert.Equal(new DateTime(2024, 8, 18, 0, 0, 0, DateTimeKind.Utc), rentEntity.StartDate);
            Assert.Equal(new DateTime(2024, 10, 1, 0, 0, 0, DateTimeKind.Utc), rentEntity.ExpectedEndDate);
            Assert.Equal(900, rentEntity.TotalPayment);
        }

        [Fact]
        public void ShouldCalculateWhenPlanIsFiftyDays()
        {
            var startDate = new DateTime(2024, 8, 17, 0, 0, 0, DateTimeKind.Utc);

            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planFiftyDays, startDate, new DateTime(2024, 10, 6, 0, 0, 0, DateTimeKind.Utc));

            Assert.Equal(new DateTime(2024, 8, 18, 0, 0, 0, DateTimeKind.Utc), rentEntity.StartDate);
            Assert.Equal(new DateTime(2024, 10, 6, 0, 0, 0, DateTimeKind.Utc), rentEntity.ExpectedEndDate);
            Assert.Equal(900, rentEntity.TotalPayment);
        }

        [Fact]
        public void ShouldRecalculateWhenRentWasCompleted()
        {
            var startDate = new DateTime(2024, 8, 17, 0, 0, 0, DateTimeKind.Utc);
            var rentEntity = new RentEntity(Guid.NewGuid(), Guid.NewGuid(), _planSevenDays, startDate, new DateTime(2024, 8, 24, 0, 0, 0, DateTimeKind.Utc));

            Assert.Equal(new DateTime(2024, 8, 18, 0, 0, 0, DateTimeKind.Utc), rentEntity.StartDate);
            Assert.Equal(new DateTime(2024, 8, 24, 0, 0, 0, DateTimeKind.Utc), rentEntity.ExpectedEndDate);
            Assert.Equal(210, rentEntity.TotalPayment);

            rentEntity.CloseRental(new DateTime(2024, 8, 25, 0, 0, 0, DateTimeKind.Utc));

            Assert.Equal(new DateTime(2024, 8, 18, 0, 0, 0, DateTimeKind.Utc), rentEntity.StartDate);
            Assert.Equal(new DateTime(2024, 8, 24, 0, 0, 0, DateTimeKind.Utc), rentEntity.ExpectedEndDate);
            Assert.Equal(new DateTime(2024, 8, 25, 0, 0, 0, DateTimeKind.Utc), rentEntity.EndDate);
            Assert.Equal(ERentStatus.Completed, rentEntity.Status);
            Assert.Equal(260, rentEntity.TotalPayment);
        }
    }
}
