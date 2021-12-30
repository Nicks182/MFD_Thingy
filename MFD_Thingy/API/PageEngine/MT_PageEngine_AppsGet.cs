using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFD_Thingy.API
{
    public partial class MT_PageEngine
    {
        

        public MT_PageInfo _Get_Apps(MT_Message P_MT_Message)
        {
            G_DB_Logic = new DB_Logic();
            return _Get_Apps();
        }

        private MT_PageInfo _Get_Apps()
        {
            return new MT_PageInfo
            {
                Caption = "Applications",
                Type = MT_PageType.LoadApps,
                BackButton = _Get_Apps_BackButton(),
                Items = _Get_Apps_Items()
            };
        }

        private MT_PageInfo_Button _Get_Apps_BackButton()
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

        private List<MT_PageInfo_Item> _Get_Apps_Items()
        {
            List<MT_PageInfo_Item> L_Items = new List<MT_PageInfo_Item>();
            List<MFD_Thingy.DB.Context.Application> L_Apps = G_DB_Logic._Get_Apps();

            for (int i = 0; i < L_Apps.Count; i++)
            {
                L_Items.Add(new MT_PageInfo_Item
                {
                    Caption = L_Apps[i].Name,
                    Id = L_Apps[i].Id.ToString(),
                    Type = MT_PageInfo_Item_Type.Button,
                    Message = new MT_Message()
                    {
                        MessageType = MT_PageType.LoadApp,
                        Data = new List<MT_Message_Data> 
                        {
                            _Get_MT_Message_Data_AppId(L_Apps[i].Id)
                        }
                    }
                });

            }

            return L_Items;
        }
    }



}
