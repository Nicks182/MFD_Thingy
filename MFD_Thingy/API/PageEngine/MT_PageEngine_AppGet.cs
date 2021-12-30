using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFD_Thingy.API
{
    public partial class MT_PageEngine
    {
        public MT_PageInfo _Get_App(MT_Message P_MT_Message)
        {
            G_DB_Logic = new DB_Logic();
            MT_Message_Data L_Data_AppId = P_MT_Message.Data.Where(d => d.Name.ToLower().Equals("appid")).FirstOrDefault();

            return _Get_App(Convert.ToInt32(L_Data_AppId.Value));
        }

        private MT_PageInfo _Get_App(int P_AppId)
        {
            MFD_Thingy.DB.Context.Application L_App = G_DB_Logic._Get_App(P_AppId);

            return new MT_PageInfo
            {
                Caption = L_App.Name,
                Type = MT_PageType.LoadApps,
                BackButton = _Get_App_BackButton(),
                Items = _Get_App_Screens(P_AppId)
            };
        }

        private MT_PageInfo_Button _Get_App_BackButton()
        {
            return new MT_PageInfo_Button
            {
                Function = "_Connection_Send",
                Caption = "B",
                EditMessage = new MT_Message
                {
                    MessageType = MT_PageType.LoadApps,
                },
            };
        }


        private List<MT_PageInfo_Item> _Get_App_Screens(int P_AppId)
        {
            List<MT_PageInfo_Item> L_Items = new List<MT_PageInfo_Item>();
            List<MFD_Thingy.DB.Context.Screen> L_Screens = G_DB_Logic._Get_Screens(P_AppId);

            for (int i = 0; i < L_Screens.Count; i++)
            {
                L_Items.Add(new MT_PageInfo_Item
                {
                    Caption = L_Screens[i].Name,
                    Id = L_Screens[i].Id.ToString(),
                    Message = new MT_Message()
                    {
                        MessageType = MT_PageType.LoadAppControls,
                        Data = new List<MT_Message_Data> 
                        {
                            _Get_MT_Message_Data_AppId(P_AppId),
                            _Get_MT_Message_Data_ScreenId(L_Screens[i].Id)
                        }
                    }
                });

            }

            return L_Items;
        }
    }



}
