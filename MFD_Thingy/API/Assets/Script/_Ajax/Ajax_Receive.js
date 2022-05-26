

function _Connection_Receive(P_MT_Message)
{
    try
    {
        console.log(P_MT_Message);

        G_ShowStatic = P_MT_Message.ShowStatic;

        if (G_ShowStatic == true)
        {
            setTimeout(function ()
            {
                _UpdatePage(P_MT_Message);
            }, 100);
        }
        else
        {
            _UpdatePage(P_MT_Message);
        }
        
    }
    catch (ex)
    {
        alert("Function: _Connection_Receive \nMsg: " + ex);
    }
}



function _UpdatePage(P_MT_Message)
{
    try
    {
        $G_Body.html(P_MT_Message.HTML);
        $G_AppStyle.html("").html(P_MT_Message.CSS);
        $G_WhiteNoise.attr("isfade", "0");
        
    }
    catch (ex)
    {
        alert("Function: _UpdatePage \nMsg: " + ex);
    }
}

