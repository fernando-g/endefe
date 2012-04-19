
function openWindows(estado, file) 
{
    w = $(window).width();
    h = $(window).height();

    var popW = 300, popH = 200;

    var leftPos = (w - popW) / 2;
    var topPos = (h - popH) / 2;
    var win = window.open("../Handlers/PdfHandler.ashx?file=" + file, 'blank', 'width=' + w + ',height=' + h + ',top=0,left=0');
    if (estado != "V") {
        win = window.open("InfoVisualizacion.aspx", 'popup', 'width=' + popW + ',height=' + popH + ',top=' + topPos + ',left=' + leftPos);
    }   

    return true;
}

function Nav(url, parameters) {
    document.location.href = url + parameters;
}