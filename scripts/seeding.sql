INSERT INTO "User" ("Id", "Identity", "Password", "Role", "CreatedAt", "UpdatedAt")
VALUES
    (gen_random_uuid(), '81216414000154', '8iqxgDnB/2tXe5G2o1WTMw==', 'Admin', NOW(), NULL),
    ('a2284da1-e871-4e57-8af1-fce994aa1824', '12345678000195', '8iqxgDnB/2tXe5G2o1WTMw==', 'Courier', NOW(), NULL);

INSERT INTO "Plan" ("Id", "Days", "DailyRate", "DailyFineRate", "CreatedAt", "UpdatedAt")
VALUES
    (gen_random_uuid(), 7, 30.0, 20.0, NOW(), NULL),
    (gen_random_uuid(), 15, 28.0, 40.0, NOW(), NULL),
    (gen_random_uuid(), 30, 22.0, 0.0, NOW(), NULL),
    (gen_random_uuid(), 45, 20.0, 0.0, NOW(), NULL),
    (gen_random_uuid(), 50, 18.0, 0.0, NOW(), NULL);

INSERT INTO "Motorcycle" ("Id", "Year", "Model", "LicensePlate", "CreatedAt", "UpdatedAt")
VALUES
    (gen_random_uuid(), 2020, 'Honda CBR600RR', 'OSW9047', NOW(), NULL),
    (gen_random_uuid(), 2019, 'Yamaha R1', 'CBA8H38', NOW(), NULL),
    (gen_random_uuid(), 2021, 'Kawasaki Ninja ZX-10R', 'LMN9101', NOW(), NULL),
    (gen_random_uuid(), 2022, 'Suzuki GSX-R1000', 'OPQ1122', NOW(), NULL),
    (gen_random_uuid(), 2023, 'Ducati Panigale V4', 'XJC6D45', NOW(), NULL);

INSERT INTO "Courier" ("Id", "Name", "Cnpj", "DrivingLicense", "DrivingLicenseCategory", "DrivingLicensePath", "BirthDate", "CreatedAt", "UpdatedAt")
VALUES
    ('a2284da1-e871-4e57-8af1-fce994aa1824', 'Elias Lima', '12345678000195', '28517540912', 'A', 'path/to/license1.png', '1985-03-15', NOW(), NULL);

INSERT INTO "Order" ("Id", "DeliveryStatus", "CreatedAt", "UpdatedAt") VALUES
    (gen_random_uuid(), 'Pending', NOW(), NULL),
    ('30eda151-20f9-4e85-b373-063b35a178fe', 'In Progress', NOW(), NULL),
    ('219024a1-1e01-4b9f-b262-aae5deae3cff', 'Completed', NOW(), NOW()),
    (gen_random_uuid(), 'Pending', NOW(), NULL),
    (gen_random_uuid(), 'Pending', NOW(), NULL),
    (gen_random_uuid(), 'Pending', NOW(), NOW()),
    (gen_random_uuid(), 'Pending', NOW(), NULL),
    (gen_random_uuid(), 'Pending', NOW(), NULL),
    (gen_random_uuid(), 'Pending', NOW(), NOW()),
    (gen_random_uuid(), 'Pending', NOW(), NULL);

INSERT INTO "DeliveryAssignment" ("Id", "CourierId", "OrderId", "AssignedAt", "CompletedAt", "CreatedAt", "UpdatedAt") VALUES
    (gen_random_uuid(), 'a2284da1-e871-4e57-8af1-fce994aa1824', '30eda151-20f9-4e85-b373-063b35a178fe', NOW(), NULL, NOW(), NULL),
    (gen_random_uuid(), 'a2284da1-e871-4e57-8af1-fce994aa1824', '219024a1-1e01-4b9f-b262-aae5deae3cff', NOW(), NOW(), NOW(), NOW());
