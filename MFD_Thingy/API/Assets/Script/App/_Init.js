
$(document).ready(function() {

    _Init();
});


function _Init() 
{

    var L_Message =
    {
        MessageType: "LoadApps"
    }

    _Connection_Send(L_Message);
}