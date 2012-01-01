using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Contracts;

namespace FacturaElectronica.Common.Services
{
    public interface IMaestrosService
    {
        List<WebServiceAfipDto> ObtenerWebServicesAfip();
    }
}
