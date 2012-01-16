
function openWindows(estado, file) 
{
    if (document.all || document.layers) 
    {
        w = screen.availWidth;
        h = screen.availHeight;
    }
    var popW = 300, popH = 200;

    var leftPos = (w - popW) / 2;
    var topPos = (h - popH) / 2;
    window.open("../Handlers/PdfHandler.ashx?file=" + file, 'blank', 'width=' + w + ',height=' + h + ',top=0,left=0');
    if (estado != "V") {
        window.open("InfoVisualizacion.aspx", 'popup', 'width=' + popW + ',height=' + popH + ',top=' + topPos + ',left=' + leftPos);
    }

    return true;
}
