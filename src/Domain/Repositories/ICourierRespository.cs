﻿using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICourierRespository
    {
        Task CreateCourier(CourierEntity entity, CancellationToken cancellationToken);
        Task UpdateCourier(CourierEntity entity, CancellationToken cancellationToken);
        Task<CourierEntity?> FindCourierById(Guid id, CancellationToken cancellationToken);
        Task<CourierEntity?> FindCourierByCnpj(string cnpj, CancellationToken cancellationToken);
        Task<CourierEntity?> FindCourierByDrivingLicense(string drivingLicense, CancellationToken cancellationToken);
    }
}
