using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Business.Services;

namespace FacturaElectronica.Ui.Win.Administrador.Code
{
    public static class ServiceFactory
    {
        public static ICorridaService GetCorridaService()
        {
            return new CorridaService();
        }

        public static IMaestrosService GetMaestroService()
        {
            return new MaestrosSevice();
        }
    }
}
