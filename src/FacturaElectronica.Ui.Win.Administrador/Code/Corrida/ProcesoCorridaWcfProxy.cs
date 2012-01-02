using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Ui.Win.Administrador.ServiceReferenceCorrida;
using FacturaElectronica.Common.Contracts.Search;


namespace FacturaElectronica.Ui.Win.Administrador.Code.Corrida
{
    public class ProcesoCorridaWcfProxy : FacturaElectronica.Common.Services.IProcesoCorridaService
    {
        public CorridaAutorizacionDto CrearNuevaCorrida(string nombreDeArchivo)
        {
            CorridaAutorizacionDto dto = null;
            ProcesoCorridaServiceClient client = new ProcesoCorridaServiceClient();
            try
            {                
                dto = client.CrearNuevaCorrida(nombreDeArchivo);
                client.Close();
            }
            catch
            {
                if (client.State != System.ServiceModel.CommunicationState.Closed)
                {
                    client.Abort();
                }

                throw;
            }

            return dto;
        }

        public void EjecutarCorrida(long corridaId)
        {
            ProcesoCorridaServiceClient client = new ProcesoCorridaServiceClient();
            try
            {
                client.EjecutarCorrida(corridaId);
                client.Close();
            }
            catch
            {
                if (client.State != System.ServiceModel.CommunicationState.Closed)
                {
                    client.Abort();
                }

                throw;
            }     
        }

        public List<CorridaAutorizacionDto> ObtenerCorridas(CorridaSearch search)
        {
            List<CorridaAutorizacionDto> dto = null;
            ProcesoCorridaServiceClient client = new ProcesoCorridaServiceClient();
            try
            {
                dto = client.ObtenerCorridas(search);
                client.Close();
            }
            catch
            {
                if (client.State != System.ServiceModel.CommunicationState.Closed)
                {
                    client.Abort();
                }

                throw;
            }

            return dto;
        }        

        public List<LogCorridaDto> ConsultarLog(long corridaId, DateTime fecha)
        {
            List<LogCorridaDto> dto = null;
            ProcesoCorridaServiceClient client = new ProcesoCorridaServiceClient();
            try
            {
                dto = client.ConsultarLog(corridaId, fecha);
                client.Close();
            }
            catch
            {
                if (client.State != System.ServiceModel.CommunicationState.Closed)
                {
                    client.Abort();
                }

                throw;
            }

            return dto;
        }
    }
}
