using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Business.Services;
using FacturaElectronica.Ui.Win.Administrador.Code.Corrida;

namespace FacturaElectronica.Ui.Win.Administrador.Code
{
    public static class ServiceFactory
    {
        public static IProcesoCorridaService GetProcesoCorridaService()
        {
            //return new CorridaService();
            return new ProcesoCorridaWcfProxy();
        }        

        public static IMaestrosService GetMaestroService()
        {
            //return new MaestrosSevice();
            return new MaestroServiceProxy();
        }

        public static IAfipWrapperService GetAfipWrapperService()
        {
            return new AfipWrapperWcfProxy();
        }
    }
}
