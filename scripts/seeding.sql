INSERT INTO "Plan" ("Id", "Days", "DailyRate", "DailyFineRate", "CreatedAt", "UpdatedAt")
VALUES
    (gen_random_uuid(), 7, 30.0, 20.0, NOW(), NULL),
    (gen_random_uuid(), 15, 28.0, 40.0, NOW(), NULL),
    (gen_random_uuid(), 30, 22.0, 0.0, NOW(), NULL),
    (gen_random_uuid(), 45, 20.0, 0.0, NOW(), NULL),
    (gen_random_uuid(), 50, 18.0, 0.0, NOW(), NULL);

INSERT INTO "Motorcycle" ("Id", "Year", "Model", "LicensePlate", "CreatedAt", "UpdatedAt")
VALUES
    (gen_random_uuid(), 2020, 'Honda CBR600RR', 'ABC1234', NOW(), NULL),
    (gen_random_uuid(), 2019, 'Yamaha R1', 'XYZ5678', NOW(), NULL),
    (gen_random_uuid(), 2021, 'Kawasaki Ninja ZX-10R', 'LMN9101', NOW(), NULL),
    (gen_random_uuid(), 2022, 'Suzuki GSX-R1000', 'OPQ1122', NOW(), NULL),
    (gen_random_uuid(), 2023, 'Ducati Panigale V4', 'RST3344', NOW(), NULL);

INSERT INTO "Courier" ("Id", "Name", "Cnpj", "DrivingLicense", "DrivingLicenseCategory", "DrivingLicensePath", "BirthDate", "CreatedAt", "UpdatedAt")
VALUES
    (gen_random_uuid(), 'João Silva', '12345678000195', '123456789', 'A', 'path/to/license1.png', '1985-03-15', NOW(), NULL),
    (gen_random_uuid(), 'Maria Oliveira', '98765432000185', '987654321', 'B', 'path/to/license2.png', '1990-07-22', NOW(), NULL),
    (gen_random_uuid(), 'Carlos Souza', '19283746000199', '456789123', 'AB', 'path/to/license3.png', '1982-11-30', NOW(), NULL),
    (gen_random_uuid(), 'Ana Costa', '56473829000172', '789456123', 'B', 'path/to/license4.png', '1995-05-25', NOW(), NULL),
    (gen_random_uuid(), 'Paulo Fernandes', '65748392000101', '321654987', 'A', 'path/to/license5.png', '1988-09-09', NOW(), NULL);
