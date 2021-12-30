

function _Connection_Send(P_MT_Message)
{
    try
    {
        $.ajax(
        {
            type: "POST",
            url: "api/MT_Message",
            data: _ToJSON(P_MT_Message),
            //data: P_App_Message,
            /*headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },*/
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (xhr, ajaxOptions, thrownError)
            {
                console.log(xhr);
                var err = eval("(" + xhr.responseText + ")");
            }
        })
        .done(function (P_MT_Message_Result)
        {
            console.log(P_MT_Message_Result);
            _Connection_Receive(P_MT_Message_Result);
        });  // End $.ajax
    }
    catch (ex)
    {
        alert("Function: _Connection_Send \nMsg: " + ex);
    }
}


function _ToJSON(P_Object)
{
    return JSON.stringify(P_Object);
}