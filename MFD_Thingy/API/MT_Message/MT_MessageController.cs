using MFD_Thingy.APP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WindowsInput;
using WindowsInput.Native;

namespace MFD_Thingy.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MT_MessageController : ControllerBase
    {
        private static MessageBusService G_MessageBusService;

        private static List<MT_Action> G_Actions = null;

        private static MT_HTMLEngine G_HTMLEngine = null;
        private static MT_PageEngine G_MT_PageEngine = null;
        private static MT_SimKey G_MT_SimKey = null;

        public MT_MessageController()
        {
            if(G_MessageBusService == null)
            {
                G_MessageBusService = App.ServiceProvider.GetService(typeof(MessageBusService)) as MessageBusService;
            }

            if (G_HTMLEngine == null)
            {
                G_HTMLEngine = new MT_HTMLEngine();
            }

            if(G_MT_PageEngine == null)
            {
                G_MT_PageEngine = new MT_PageEngine();
            }

            if(G_MT_SimKey == null)
            {
                G_MT_SimKey = new MT_SimKey();
            }
        }

        [HttpPost]
        public MT_Message Post([FromBody] MT_Message P_MT_Message)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            switch (P_MT_Message.MessageType)
            {
                case MT_PageType.LoadApps:
                    G_HTMLEngine._RenderPage(P_MT_Message, G_MT_PageEngine._Get_Apps(P_MT_Message));
                    break;

                case MT_PageType.LoadApp:
                    G_HTMLEngine._RenderPage(P_MT_Message, G_MT_PageEngine._Get_App(P_MT_Message));
                    break;

                case MT_PageType.LoadAppControls:
                    G_HTMLEngine._RenderPage(P_MT_Message, G_MT_PageEngine._Get_Screen(P_MT_Message));
                    break;

                case MT_PageType.DoAction:
                    G_MT_SimKey._SimInput(P_MT_Message);
                    if(P_MT_Message.MessageType == MT_PageType.LoadAppControls)
                    {
                        G_HTMLEngine._RenderPage(P_MT_Message, G_MT_PageEngine._Get_Screen(P_MT_Message));
                    }
                    break;
            }

            watch.Stop();
            ;

            G_MessageBusService.Emit("controllermessage", P_MT_Message.MessageType.ToString() + _GetTime (watch.ElapsedMilliseconds));

            return P_MT_Message;
        }

        private string _GetTime(long P_Milliseconds)
        {
            if(P_Milliseconds > 999)
            {
                return " - " + (P_Milliseconds / 1000) + "sec";
            }
            else if (P_Milliseconds > 59000)
            {
                return " - " + ((P_Milliseconds / 1000) / 60) + "min";
            }
            else
            {
                return " - " + P_Milliseconds + "ms";
            }
        }
    }


}
