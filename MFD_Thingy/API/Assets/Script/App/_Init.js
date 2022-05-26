
var $G_Body = null;
var $G_AppStyle = null;
var $G_WhiteNoise = null;
var G_ShowStatic = false;

$(document).ready(function() {

    _Init();
});


function _Init() 
{
    $G_Body = $("#Body");
    $G_AppStyle = $("#App_Style");
    $G_WhiteNoise = $("#Div_WhiteNoise");

    var L_Message =
    {
        MessageType: "LoadApps"
    }

    _Connection_Send(L_Message);
}