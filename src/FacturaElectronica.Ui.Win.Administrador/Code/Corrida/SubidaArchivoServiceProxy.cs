using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Ui.Win.Administrador.ServiceReferenceCorrida;
using FacturaElectronica.Common.Contracts.Search;
using System.ServiceModel.Description;
using FacturaElectronica.Ui.Win.Administrador.ServiceReferenceSubidaArchivo;


namespace FacturaElectronica.Ui.Win.Administrador.Code.Corrida
{
    public class SubidaArchivoServiceProxy : FacturaElectronica.Common.Services.ISubidaArchivoService
    {

        public CorridaSubidaArchivoDto CrearNuevaCorrida()
        {
            CorridaSubidaArchivoDto dto = null;
            SubidaArchivoServiceClient client = new SubidaArchivoServiceClient();
            ClientCredentialHelper.SetCredentials(client.ClientCredentials);
            try
            {
                dto = client.CrearNuevaCorrida();
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

        public void EjecutarCorrida(long corridaId, List<string> files)
        {
            CorridaSubidaArchivoDto dto = null;
            SubidaArchivoServiceClient client = new SubidaArchivoServiceClient();
            ClientCredentialHelper.SetCredentials(client.ClientCredentials);
            try
            {
                client.EjecutarCorrida(corridaId, files);
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

        public List<CorridaSubidaArchivoDto> ObtenerCorridas(CorridaSubidaArchivoSearch search)
        {
            List<CorridaSubidaArchivoDto> dto = null;
            SubidaArchivoServiceClient client = new SubidaArchivoServiceClient();
            ClientCredentialHelper.SetCredentials(client.ClientCredentials);
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

    }
}
