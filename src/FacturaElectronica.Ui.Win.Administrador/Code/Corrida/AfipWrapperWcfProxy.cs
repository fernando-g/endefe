using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Afip.Ws.Wsfe;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Ui.Win.Administrador.ServiceReferenceAfipWrapper;

namespace FacturaElectronica.Ui.Win.Administrador.Code.Corrida
{
    public class AfipWrapperWcfProxy : FacturaElectronica.Common.Services.IAfipWrapperService
    {
        public MonedaResponse GetTiposMonedas(FEAuthRequest feAuthRequest)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.GetTiposMonedas(feAuthRequest);
                client.Close();
                return t;
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


        public FECotizacionResponse GetCotizacion(FEAuthRequest feAuthRequest, string MonId)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.GetCotizacion(feAuthRequest, MonId);
                client.Close();
                return t;
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


        public CbteTipoResponse GetTiposCbte(FEAuthRequest feAuthRequest)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.GetTiposCbte(feAuthRequest);
                client.Close();
                return t;
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


        public FETributoResponse GetTiposTributos(FEAuthRequest feAuthRequest)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.GetTiposTributos(feAuthRequest);
                client.Close();
                return t;
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


        public IvaTipoResponse GetTiposIva(FEAuthRequest feAuthRequest)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.GetTiposIva(feAuthRequest);
                client.Close();
                return t;
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


        public OpcionalTipoResponse GetTiposOpcional(FEAuthRequest feAuthRequest)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.GetTiposOpcional(feAuthRequest);
                client.Close();
                return t;
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


        public FERegXReqResponse CompTotXRequest(FEAuthRequest feAuthRequest)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.CompTotXRequest(feAuthRequest);
                client.Close();
                return t;
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


        public DocTipoResponse GetTiposDoc(FEAuthRequest feAuthRequest)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.GetTiposDoc(feAuthRequest);
                client.Close();
                return t;
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


        public FERecuperaLastCbteResponse CompUltimoAutorizado(FEAuthRequest feAuthRequest, int ptoVta, int cbteTipo)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.CompUltimoAutorizado(feAuthRequest, ptoVta, cbteTipo);
                client.Close();
                return t;
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


        public FECompConsultaResponse CompConsultar(FEAuthRequest feAuthRequest, FECompConsultaReq feCompConsultaReq)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.CompConsultar(feAuthRequest, feCompConsultaReq);
                client.Close();
                return t;
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


        public ConceptoTipoResponse GetTiposConcepto(FEAuthRequest feAuthRequest)
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.GetTiposConcepto(feAuthRequest);
                client.Close();
                return t;
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


        public FEAuthRequest ObtenerTicket()
        {
            AfipWrapperServiceClient client = new AfipWrapperServiceClient();
            try
            {
                var t = client.ObtenerTicket();
                client.Close();
                return t;
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
    }
}
