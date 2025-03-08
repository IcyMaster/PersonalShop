using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalShop.Domain.Entities.Orders;

public enum OrderStatus
{
    Completed,
    InProgress,
    Cancelled,
    NoStatus,
}
