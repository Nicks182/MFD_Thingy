

function _Connection_Receive(P_MT_Message)
{
    try
    {
        console.log(P_MT_Message);
        $("#Body").html(P_MT_Message.HTML);
        $("#App_Style").html(P_MT_Message.CSS);
        //$("head").remove("#AppStyle").append("<style id=\"App_Style\">" + P_MT_Message.CSS + "</style>");
        
    }
    catch (ex)
    {
        alert("Function: _Connection_Receive \nMsg: " + ex);
    }
}

