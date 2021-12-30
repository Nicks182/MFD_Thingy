using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFD_Thingy.API
{
    public partial class MT_PageEngine
    {


        public MT_PageInfo _Get_Screen(MT_Message P_MT_Message)
        {
            G_DB_Logic = new DB_Logic();
            MT_Message_Data L_Data_ScreenId = P_MT_Message.Data.Where(d => d.Name.ToLower().Equals("screenid")).FirstOrDefault();

            return _Get_Screen(Convert.ToInt32(L_Data_ScreenId.Value));
        }

        private MT_PageInfo _Get_Screen(int P_ScreenId)
        {
            DB.Context.Screen L_Screen = G_DB_Logic._Get_Screen(P_ScreenId);

            return new MT_PageInfo
            {
                Caption = L_Screen.Name,
                Type = MT_PageType.LoadAppControls,
                BackButton = _Get_Screen_BackButton(L_Screen.AppId.Value),
                Items = _Get_Screen_Items(P_ScreenId)
            };
        }

        private MT_PageInfo_Button _Get_Screen_BackButton(int P_AppId)
        {
            return new MT_PageInfo_Button
            {
                Function = "_Connection_Send",
                Caption = "B",
                EditMessage = new MT_Message
                {
                    MessageType = MT_PageType.LoadApp,
                    Data = new List<MT_Message_Data>
                    {
                        _Get_MT_Message_Data_AppId(P_AppId)
                    }
                },
            };
        }

        private List<MT_PageInfo_Item> _Get_Screen_Items(int P_ScreenId)
        {
            List<MT_PageInfo_Item> L_Items = new List<MT_PageInfo_Item>();
            List<MFD_Thingy.DB.Context.Screen_Control> L_DBItems = G_DB_Logic._Get_Screen_Controls(P_ScreenId);

            for (int i = 0; i < L_DBItems.Count; i++)
            {
                L_Items.Add(new MT_PageInfo_Item
                {
                    Caption = L_DBItems[i].Name,
                    Id = L_DBItems[i].Id.ToString(),
                    Type = MT_PageInfo_Item_Type.Button,
                    Message = new MT_Message()
                    {
                        MessageType = MT_PageType.DoAction,
                        Data = new List<MT_Message_Data>
                        {
                            _Get_MT_Message_Data_ControlId(L_DBItems[i].Id)
                        }
                    }
                });

            }

            return L_Items;
        }
    }



}
