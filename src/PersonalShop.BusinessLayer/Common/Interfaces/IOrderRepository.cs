﻿using PersonalShop.Domain.Entities.Orders;

namespace PersonalShop.BusinessLayer.Common.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderDetailsAsync(int orderId, bool track = true);
    }
}