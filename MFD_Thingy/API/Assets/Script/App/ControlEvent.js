

function _OnEvent(P_ActionName)
{
    var L_Message =
    {
        Id: "",
        ActionName: P_ActionName,
        WasMatched: false
    };

    _Connection_Send(L_Message);
}
