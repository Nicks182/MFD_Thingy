using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace MFD_Thingy.API
{
    public class MT_Message
    {
        public MT_PageType MessageType { get; set; }
        public List<MT_Message_Data> Data { get; set; }
        public string HTML { get; set; }
        public string CSS { get; set; }
    }

    public class MT_Message_Data
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public enum MT_PageType
    {
        ThemeLookup,
        LoadApps,
        LoadThemes,
        LoadApp,
        LoadAppControls,
        LoadActions,

        NewApp,
        NewTheme,
        NewScreen,
        NewAction,
        NewItem,

        SaveNewApp,
        SaveNewTheme,
        SaveNewScreen,
        SaveNewAction,
        SaveNewItem,

        EditApp,
        EditTheme,
        EditThemes,
        EditScreen,
        EditScreens,
        EditAction,
        EditItem,

        SaveEditApp,
        SaveEditTheme,
        SaveEditScreen,
        SaveEditAction,
        SaveEditItem,

        DoAction,
    }

    public class MT_Action
    {
        public string Id { get; set; }
        public string ActionName { get; set; }
        public int HoldTime { get; set; }
        public VirtualKeyCode KeyCode { get; set; }
    }
}
