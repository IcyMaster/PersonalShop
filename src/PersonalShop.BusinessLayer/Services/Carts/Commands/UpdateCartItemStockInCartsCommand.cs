using PersonalShop.BusinessLayer.Services.Carts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalShop.BusinessLayer.Services.Carts.Commands;

public class UpdateCartItemStockInCartsCommand : IUpdateCartItemStockInCartsCommand
{
    public int ProductId { get; set; }
    public int Stock { get; set; } = 0;
}
