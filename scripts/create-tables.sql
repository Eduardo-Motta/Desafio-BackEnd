CREATE TABLE "User" (
    "Id" UUID PRIMARY KEY,
    "Identity" VARCHAR(14) NOT NULL,
    "Password" VARCHAR(255) NOT NULL,
    "Role" VARCHAR(20) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP
);

CREATE TABLE "Motorcycle" (
    "Id" UUID PRIMARY KEY,
    "Year" INTEGER NOT NULL,
    "Model" VARCHAR(200) NOT NULL,
    "LicensePlate" VARCHAR(20) NOT NULL UNIQUE,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP
);

CREATE UNIQUE INDEX "IX_LicensePlate" ON "Motorcycle" ("LicensePlate");

CREATE TABLE "Courier" (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Cnpj" VARCHAR(14) NOT NULL,
    "DrivingLicense" VARCHAR(11) NOT NULL,
    "DrivingLicenseCategory" VARCHAR(2) NOT NULL,
    "DrivingLicensePath" TEXT,
    "BirthDate" DATE NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP
);

CREATE TABLE "Plan" (
    "Id" UUID PRIMARY KEY,
    "Days" INTEGER NOT NULL,
    "DailyRate" DECIMAL(18, 2) NOT NULL,
    "DailyFineRate" DECIMAL(18, 2) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP
);

CREATE TABLE "Rent" (
    "Id" UUID PRIMARY KEY,
    "CourierId" UUID NOT NULL,
    "PlanId" UUID NOT NULL,
    "MotorcycleId" UUID NOT NULL,
    "StartDate" TIMESTAMP NOT NULL,
    "ExpectedEndDate" TIMESTAMP NOT NULL,
    "EndDate" TIMESTAMP,
    "Status" VARCHAR(50) NOT NULL,
    "TotalPayment" DECIMAL(18, 2) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,

    CONSTRAINT "FK_Courier"
      FOREIGN KEY("CourierId")
	  REFERENCES "Courier"("Id"),

    CONSTRAINT "FK_Plan"
      FOREIGN KEY("PlanId")
	  REFERENCES "Plan"("Id"),

    CONSTRAINT "FK_Motorcycle"
      FOREIGN KEY("MotorcycleId")
	  REFERENCES "Motorcycle"("Id")
);

CREATE TABLE "RentalClosure" (
    "Id" UUID PRIMARY KEY,
    "RentId" UUID NOT NULL,
    "EndDate" TIMESTAMP,
    "ExceededDays" INTEGER NOT NULL,
    "PenaltyAmountForUnusedDay" DECIMAL(18, 2) NOT NULL,
    "CostForUsedDays" DECIMAL(18, 2) NOT NULL,
    "TotalAdditionalDailyAmount" DECIMAL(18, 2) NOT NULL,
    "TotalPayment" DECIMAL(18, 2) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,

    CONSTRAINT "FK_Rent"
      FOREIGN KEY("RentId")
	  REFERENCES "Rent"("Id")
);

CREATE TABLE "Order" (
    "Id" UUID PRIMARY KEY,
    "DeliveryStatus" VARCHAR(20) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP
);

CREATE TABLE "DeliveryAssignment" (
    "Id" UUID PRIMARY KEY,
    "CourierId" UUID NOT NULL,
    "OrderId" UUID NOT NULL,
    "AssignedAt" TIMESTAMP NOT NULL,
    "CompletedAt" TIMESTAMP,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,

    CONSTRAINT "FK_Courier"
      FOREIGN KEY("CourierId")
	  REFERENCES "Courier"("Id"),

    CONSTRAINT "FK_Order"
      FOREIGN KEY("OrderId")
	  REFERENCES "Order"("Id")
);

CREATE TABLE "MotorcycleNotification" (
    "Id" UUID,
    "Year" INTEGER NOT NULL,
    "Model" VARCHAR(200) NOT NULL,
    "LicensePlate" VARCHAR(20) NOT NULL
);
