using MotosRental.Entities;

namespace MotosRental.Interfaces;

public interface INotificationRepository
{
    public Task AddAsync(Notification notification);
}