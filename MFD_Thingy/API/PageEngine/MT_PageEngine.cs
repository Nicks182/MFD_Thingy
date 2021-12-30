using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFD_Thingy.API
{
    public partial class MT_PageEngine
    {
        private DB_Logic G_DB_Logic = null;

        public MT_PageEngine()
        {
        }

        private MT_Message_Data _Get_MT_Message_Data_AppId(int P_AppId)
        {
            return new MT_Message_Data
            {
                Name = "AppId",
                Value = P_AppId.ToString()
            };
        }

        private MT_Message_Data _Get_MT_Message_Data_ScreenId(int P_ScreenId)
        {
            return new MT_Message_Data
            {
                Name = "ScreenId",
                Value = P_ScreenId.ToString()
            };
        }

        private MT_Message_Data _Get_MT_Message_Data_ControlId(int P_ControlId)
        {
            return new MT_Message_Data
            {
                Name = "ControlId",
                Value = P_ControlId.ToString()
            };
        }

    }


    public class MT_PageInfo
    {
        public string Caption { get; set; }
        public MT_PageType Type { get; set; }
        public List<MT_PageInfo_Item> Items { get; set; }

        public MT_PageInfo_Button BackButton { get; set; }
    }

    public class MT_PageInfo_Button
    {
        public string Function { get; set; }
        public string Caption { get; set; }
        public MT_Message EditMessage { get; set; }
    }

    public class MT_PageInfo_Item
    {
        public string Id { get; set; }
        public string Caption { get; set; }
        public string Value { get; set; }
        public MT_PageInfo_Item_Type Type { get; set; }
        public MT_Message Message { get; set; }
    }


    public enum MT_PageInfo_Item_Type
    {
        Button,
        Text,
        Check,
        Lookup,
        PickColor
    }

}
