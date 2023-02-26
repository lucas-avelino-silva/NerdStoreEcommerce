﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Pedidos.Domain
{
    public enum PedidoStatus
    {
        Autorizado = 1,
        Pago = 2,
        Recusado = 3,
        Entregue = 4,
        Cancelado = 5
    }
}
