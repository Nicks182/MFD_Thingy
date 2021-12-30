using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MFD_Thingy.API
{
    public partial class MT_HTMLEngine
    {
        private DB_Logic G_DB_Logic = null;
        public MT_HTMLEngine()
        {
        }

        public void _RenderPage(MT_Message P_MT_Message, MT_PageInfo P_MT_PageInfo)
        {
            G_DB_Logic = new DB_Logic();
            StringBuilder L_Html = new StringBuilder();

            L_Html.Append(
                "" +
                    "<div " +
                        "class=\"Page\" " +
                    ">" +

                        _RenderPage_Button("Page_Back", P_MT_PageInfo.BackButton) +

                        _Render_Button(new HTML_ControlInfo
                        {
                            Class = "Page_Caption MT_Btn",
                            Caption = P_MT_PageInfo.Caption,
                            //Event = _GetFullScreenEvent()
                        }) +

                        _Render_Button(new HTML_ControlInfo
                        {
                            Class = "Page_Fulscreen MT_Btn",
                            Caption = "F",
                            Event = _GetFullScreenEvent()
                        }) +

                        "<div " +
                            "class=\"Page_Body\" " +
                        ">" +
                            _RenderPage_Items(P_MT_PageInfo.Items) +
                        "</div>" +

                    "</div>" +
                "");

            P_MT_Message.HTML = L_Html.ToString();
            _Generate_CSS(P_MT_Message);
        }

        private string _RenderPage_Button(string P_Class, MT_PageInfo_Button P_MT_PageInfo_EditButton)
        {
            if (P_MT_PageInfo_EditButton != null)
            {
                return
                    _Render_Button(new HTML_ControlInfo
                    {
                        Class = P_Class + " MT_Btn",
                        Caption = P_MT_PageInfo_EditButton.Caption,
                        Event = _GenerateEvent(P_MT_PageInfo_EditButton.Function, P_MT_PageInfo_EditButton.EditMessage)
                    }).ToString();
            }

            return "";
        }

        public StringBuilder _RenderPage_Items(List<MT_PageInfo_Item> P_MT_PageInfo_Items)
        {
            StringBuilder L_Html = new StringBuilder();

            for (int i = 0; i < P_MT_PageInfo_Items.Count; i++)
            {
                L_Html.Append(_RenderPage_Item_Button(P_MT_PageInfo_Items[i]));
            }
            return L_Html;
        }

        public void _Generate_CSS(MT_Message P_MT_Message)
        {
            switch(P_MT_Message.MessageType)
            {
                case MT_PageType.LoadAppControls:
                    _Generate_CSS_Screen(P_MT_Message);
                    break;

                default:
                    P_MT_Message.CSS = string.Empty;
                    break;
            }


            
        }

        public void _Generate_CSS_Screen(MT_Message P_MT_Message)
        {
            StringBuilder L_Css = new StringBuilder();

            MT_Message_Data L_Data_AppId = P_MT_Message.Data.Where(d => d.Name.ToLower().Equals("appid")).FirstOrDefault();
            MT_Message_Data L_Data_ScreenId = P_MT_Message.Data.Where(d => d.Name.ToLower().Equals("screenid")).FirstOrDefault();

            DB.Context.Screen L_Screen = G_DB_Logic._Get_Screen(Convert.ToInt32(L_Data_ScreenId.Value));
            DB.Context.Theme L_Theme = null;

            if (L_Screen.ThemeId != null)
            {
                L_Theme = G_DB_Logic._Get_Theme(L_Screen.ThemeId.Value);
                if (L_Theme != null)
                {
                    L_Css.Append(_Generate_CSS_Screen_Item(new MT_Screen_CSS_Item
                    {
                        ColorBG     = MT_Utils._ConvertStringToColor(L_Theme.Color_ScreenBackground, MT_Utils.G_DefaultScreenBackground),
                        Spacing     = MT_Utils._GetDefaultSpacing(L_Theme.Item_Spacing).ToString() + "px",
                        BtnColor    = MT_Utils._ConvertStringToColor(L_Theme.Color_Forground, MT_Utils.G_DefaultForground),
                        MinWidth    = MT_Utils._GetDefaultMinWidth(L_Theme.Item_MinWidth).ToString() + "px",
                        ClassName   = "html",
                        BtnColorBG  = MT_Utils._ConvertStringToColor(L_Theme.Color_Background, MT_Utils.G_DefaultBackground),
                        MainBGColor = L_Theme.Color_ScreenBackground,
                    })) ;
                }
            }

            foreach (var L_Control in G_DB_Logic._Get_Screen_Controls(L_Screen.Id).Where(c => c.ThemeId != null).ToList())
            {
                L_Theme = G_DB_Logic._Get_Theme(L_Control.ThemeId.Value);
                if (L_Theme != null)
                {
                    L_Css.Append(_Generate_CSS_Screen_Item(new MT_Screen_CSS_Item
                    {
                        ColorBG     = MT_Utils._ConvertStringToColor(L_Theme.Color_ScreenBackground, MT_Utils.G_DefaultScreenBackground),
                        Spacing     = MT_Utils._GetDefaultSpacing(L_Theme.Item_Spacing).ToString() + "px",
                        BtnColor    = MT_Utils._ConvertStringToColor(L_Theme.Color_Forground, MT_Utils.G_DefaultForground),
                        MinWidth    = MT_Utils._GetDefaultMinWidth(L_Theme.Item_MinWidth).ToString() + "px",
                        ClassName   = "#Btn_" + L_Control.Id.ToString(),
                        BtnColorBG  = MT_Utils._ConvertStringToColor(L_Theme.Color_Background, MT_Utils.G_DefaultBackground),
                        MainBGColor = L_Theme.Color_ScreenBackground,
                    }));
                }
            }

            P_MT_Message.CSS = L_Css.ToString();
            
        }

        private string _Generate_CSS_Screen_Item(MT_Screen_CSS_Item P_MT_Screen_CSS_Item)
        {
            return
            "" +
                P_MT_Screen_CSS_Item.ClassName + Environment.NewLine +
                "{" + Environment.NewLine +
                    "--MainBackgroundColor: " + P_MT_Screen_CSS_Item.MainBGColor + " !important;" + Environment.NewLine +
                    "--BtnColor:  rgba(" + P_MT_Screen_CSS_Item.BtnColor.R + ", " + P_MT_Screen_CSS_Item.BtnColor.G + ", " + P_MT_Screen_CSS_Item.BtnColor.B + ", 1) !important; " + Environment.NewLine +
                    "--BtnColorBG:  rgba(" + P_MT_Screen_CSS_Item.BtnColorBG.R + ", " + P_MT_Screen_CSS_Item.BtnColorBG.G + ", " + P_MT_Screen_CSS_Item.BtnColorBG.B + ", 0.1) !important; " + Environment.NewLine +
                    "--ColorBG:  rgba(" + P_MT_Screen_CSS_Item.ColorBG.R + ", " + P_MT_Screen_CSS_Item.ColorBG.G + ", " + P_MT_Screen_CSS_Item.ColorBG.B + ", 1) !important; " + Environment.NewLine +
                    "--ControlSpacing:  " + P_MT_Screen_CSS_Item.Spacing + " !important; " + Environment.NewLine +
                    "--ControlMinWidth:  " + P_MT_Screen_CSS_Item.MinWidth + " !important; " + Environment.NewLine +
                    "--BoxShadow: rgba(" + P_MT_Screen_CSS_Item.BtnColor.R + ", " + P_MT_Screen_CSS_Item.BtnColor.G + ", " + P_MT_Screen_CSS_Item.BtnColor.B + ", .4) !important; " + Environment.NewLine +
            "}" + Environment.NewLine +
            "";
        }

        public class MT_Screen_CSS_Item
        {
            public string ClassName { get; set; }
            public string MainBGColor { get; set; }
            public Color BtnColor { get; set; }
            public Color BtnColorBG { get; set; }
            public Color ColorBG { get; set; }
            public string Spacing { get; set; }
            public string MinWidth { get; set; }
        }

        //public string _RenderPage_Item(MT_PageInfo_Item P_MT_PageInfo_Item)
        //{
        //    switch(P_MT_PageInfo_Item.Type)
        //    {
        //        case MT_PageInfo_Item_Type.Button:
        //            return _RenderPage_Item_Button(P_MT_PageInfo_Item);

        //        case MT_PageInfo_Item_Type.Text:
        //        case MT_PageInfo_Item_Type.Lookup:
        //            return _RenderPage_Item_Text(P_MT_PageInfo_Item);

        //        case MT_PageInfo_Item_Type.Check:
        //            return _RenderPage_Item_Check(P_MT_PageInfo_Item);

        //    }

        //    return "";
        //}

        public string _RenderPage_Item_Button(MT_PageInfo_Item P_MT_PageInfo_Item)
        {
            return
                _Render_Button(new HTML_ControlInfo
                {
                    Id = P_MT_PageInfo_Item.Id,
                    Class = "MT_Btn",
                    Caption = P_MT_PageInfo_Item.Caption,
                    Event = _GenerateEvent(null, P_MT_PageInfo_Item.Message)
                }).ToString();
        }

        //public string _RenderPage_Item_Text(MT_PageInfo_Item P_MT_PageInfo_Item)
        //{
        //    return
        //        _Render_TextBox(new HTML_ControlInfo
        //        {
        //            Id = P_MT_PageInfo_Item.Id,
        //            Caption = P_MT_PageInfo_Item.Caption,
        //            Class = "MT_HtmlControl",
        //            IsReadonly = P_MT_PageInfo_Item.Type == MT_PageInfo_Item_Type.Lookup,
        //            Value = P_MT_PageInfo_Item.Value,
        //            Event = _GenerateEvent(null, P_MT_PageInfo_Item.Message)
        //        });
        //}

        //public string _RenderPage_Item_Check(MT_PageInfo_Item P_MT_PageInfo_Item)
        //{
        //    return
        //        _Render_CheckBox(new HTML_ControlInfo
        //        {
        //            Id = P_MT_PageInfo_Item.Id,
        //            Caption = P_MT_PageInfo_Item.Caption,
        //            Class = "MT_HtmlControl",
        //            Value = P_MT_PageInfo_Item.Value
        //        });
        //}

        //public string _RenderPage_Item_Lookup(MT_PageInfo_Item P_MT_PageInfo_Item)
        //{
        //    return
        //        _Render_Lookup(new HTML_ControlInfo
        //        {
        //            Id = P_MT_PageInfo_Item.Id,
        //            Caption = P_MT_PageInfo_Item.Caption,
        //            Class = "MT_HtmlControl",
        //            Value = P_MT_PageInfo_Item.Value
        //        });
        //}

        public string _GenerateEvent(string P_Function, MT_Message P_MT_Message)
        {
            if (P_MT_Message != null)
            {
                return
                "" +
                    _GenerateEvent_Function(P_Function) +
                    "(" +
                        MT_Utils._ToJson(P_MT_Message).Replace("\"", "'") +
                    ")" +
                "";
            }

            return "";
        }

        public string _GenerateEvent_Function(string P_Function)
        {
            if(P_Function == null || P_Function == string.Empty)
            {
                return "_Connection_Send";
            }
            return P_Function;
        }



        //private StringBuilder _Gen_Page(HTML_PageInfo P_HTML_PageInfo)
        //{
        //    StringBuilder L_Html = new StringBuilder();

        //    L_Html.Append(
        //        "" +
        //            "<div " +
        //                "class=\"Page\" " +
        //            ">" +

        //                _Gen_Button(new HTML_ButtonInfo
        //                {
        //                    Class = "Page_Back",
        //                    Caption = "B",
        //                    Event = ""
        //                }) +

        //                _Gen_Button(new HTML_ButtonInfo
        //                {
        //                    Class = "Page_Caption",
        //                    Caption = P_HTML_PageInfo.Caption,
        //                    Event = _GetFullScreenEvent()
        //                }) +

        //                "<div " +
        //                    "class=\"Page_Body\" " +
        //                ">" +
        //                    P_HTML_PageInfo.Body +
        //                "</div>" +

        //            "</div>" +
        //        "");

        //    return L_Html;
        //}

        private string _Render_Button(HTML_ControlInfo P_HTML_Button)
        {
            return
                "" +
                    "<button " +
                        "id=\"Btn_" + P_HTML_Button.Id + "\" " +
                        "type=\"button\" " +
                        "class=\"" + P_HTML_Button.Class + "\" " +
                        "onClick=\"" + P_HTML_Button.Event + "\" " +
                    ">" +
                        P_HTML_Button.Caption +
                    "</button>" +
                "";
        }

        //private string _Render_TextBox(HTML_ControlInfo P_HTML_TextInfo)
        //{
        //    return
        //    "" +
        //        "<div class=\"" + P_HTML_TextInfo.Class + "\" >" +
        //            "<label>" +
        //                P_HTML_TextInfo.Caption + ": " +
        //            "</label>" +
        //            "<input " +
        //                "id=\"" + P_HTML_TextInfo.Id + "\" " +
        //                "type=\"text\" " +
        //                "class=\"\" " +
        //                "value=\"" + P_HTML_TextInfo.Value + "\" " +
        //                _Render_TextBox_Readonly(P_HTML_TextInfo.IsReadonly) +
        //                _Render_TextBox_Event(P_HTML_TextInfo.Event) +
        //            "/>" +
        //        "</div>" +
        //    "";
        //}

        //private string _Render_TextBox_Readonly(bool P_IsReadonly)
        //{
        //    if(P_IsReadonly == true)
        //    {
        //        return " readonly ";
        //    }
        //    return "";
        //}

        //private string _Render_TextBox_Event(string P_Event)
        //{
        //    if (P_Event != null && P_Event != string.Empty)
        //    {
        //        return "onClick=\"" + P_Event + "\" ";
        //    }
        //    return "";
        //}

        //private string _Render_CheckBox(HTML_ControlInfo P_HTML_TextInfo)
        //{
        //    return
        //    "" +
        //        "<div class=\"" + P_HTML_TextInfo.Class + "\" >" +
        //            "<label>" +
        //                P_HTML_TextInfo.Caption + ": " +
        //            "</label>" +
        //            "<input " +
        //                "id=\"" + P_HTML_TextInfo.Id + "\" " +
        //                "type=\"checkbox\" " +
        //                "class=\"\" " +
        //                _Render_CheckBox_IsChecked(P_HTML_TextInfo.Value) + 
        //            "/>" +
        //        "</div>" +
        //    "";
        //}

        //private string _Render_Lookup(HTML_ControlInfo P_HTML_TextInfo)
        //{
        //    return
        //    "" +
        //        "<div class=\"" + P_HTML_TextInfo.Class + "\" >" +
        //            "<label>" +
        //                P_HTML_TextInfo.Caption + ": " +
        //            "</label>" +
        //            "<input " +
        //                "id=\"" + P_HTML_TextInfo.Id + "\" " +
        //                "type=\"button\" " +
        //                "class=\"MT_Btn\" " +
        //                _Render_CheckBox_IsChecked(P_HTML_TextInfo.Value) +
        //            "/>" +
        //        "</div>" +
        //    "";
        //}

        //private string _Render_CheckBox_IsChecked(string P_Value)
        //{
        //    if(P_Value != null && P_Value != string.Empty && Convert.ToBoolean(P_Value) == true)
        //    {
        //        return " checked ";
        //    }

        //    return "";
        //}

        private string _GetFullScreenEvent()
        {
            return "_SetFullscreen();";
        }
    }

    public class HTML_ControlInfo
    {
        public string Id { get; set; }
        public string Caption { get; set; }
        public string Class { get; set; }
        public string Value { get; set; }
        public string Event { get; set; }
        public bool IsReadonly { get; set; }
    }


    public class HTML_PageInfo
    {
        public string Id { get; set; }
        public string Caption { get; set; }
        public string ParentId { get; set; }
        public StringBuilder Body { get; set; }
    }

}
