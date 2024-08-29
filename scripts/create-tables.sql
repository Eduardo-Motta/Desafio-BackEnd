CREATE TABLE Motorcycle (
    "Id" UUID PRIMARY KEY,
    "Year" INTEGER NOT NULL,
    "Model" VARCHAR(200) NOT NULL,
    "LicensePlate" VARCHAR(20) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP
);

CREATE UNIQUE INDEX "IX_LicensePlate" ON Motorcycle ("LicensePlate");

CREATE TABLE "Courier" (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Cnpj" VARCHAR(14) NOT NULL,
    "DrivingLicense" VARCHAR(9) NOT NULL,
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
