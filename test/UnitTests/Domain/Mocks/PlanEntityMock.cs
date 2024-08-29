using Domain.Entities;

namespace UnitTests.Domain.Mocks
{
    public class PlanEntityMock
    {
        public PlanEntity PlanSevenDays { get; private set; }
        public PlanEntity PlanFifteenDays { get; private set; }
        public PlanEntity PlanThirtyDays { get; private set; }
        public PlanEntity PlanFortyFiveDays { get; private set; }
        public PlanEntity PlanFiftyDays { get; private set; }

        public PlanEntityMock()
        {
            PlanSevenDays = new PlanEntity(7, 30.0m, 20.0m);
            PlanFifteenDays = new PlanEntity(15, 28.0m, 40.0m);
            PlanThirtyDays = new PlanEntity(30, 22.0m, 0);
            PlanFortyFiveDays = new PlanEntity(45, 20.0m, 0);
            PlanFiftyDays = new PlanEntity(50, 18.0m, 0);
        }
    }
}
