using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using FacturaElectronica.Afip.Ws.Wsfe;

namespace FacturaElectronica.Common.Services
{
    [ServiceContract]
    public interface IAfipWrapperService
    {
        [OperationContract]
        MonedaResponse GetTiposMonedas(FEAuthRequest feAuthRequest);

        [OperationContract]
        FECotizacionResponse GetCotizacion(FEAuthRequest feAuthRequest, string MonId);

        [OperationContract]
        CbteTipoResponse GetTiposCbte(FEAuthRequest feAuthRequest);

        [OperationContract]
        FETributoResponse GetTiposTributos(FEAuthRequest feAuthRequest);

        [OperationContract]
        IvaTipoResponse GetTiposIva(FEAuthRequest feAuthRequest);

        [OperationContract]
        OpcionalTipoResponse GetTiposOpcional(FEAuthRequest feAuthRequest);

        [OperationContract]
        FERegXReqResponse CompTotXRequest(FEAuthRequest feAuthRequest);

        [OperationContract]
        DocTipoResponse GetTiposDoc(FEAuthRequest feAuthRequest);

        [OperationContract]
        FERecuperaLastCbteResponse CompUltimoAutorizado(FEAuthRequest feAuthRequest, int ptoVta, int cbteTipo);

        [OperationContract]
        FECompConsultaResponse CompConsultar(FEAuthRequest feAuthRequest, FECompConsultaReq feCompConsultaReq);

        [OperationContract]
        ConceptoTipoResponse GetTiposConcepto(FEAuthRequest feAuthRequest);

        [OperationContract]
        FEAuthRequest ObtenerTicket();
    }
}
