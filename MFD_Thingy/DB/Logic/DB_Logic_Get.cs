using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MFD_Thingy.API;
using MFD_Thingy.DB.Context;

namespace MFD_Thingy
{
    public partial class DB_Logic
    {
        public List<DB.Context.Application> _Get_Apps()
        {
            return G_DB_ThingyContext.Applications.ToList();
        }

        public DB.Context.Application _Get_App(int P_AppId)
        {
            return G_DB_ThingyContext.Applications.Where(a => a.Id == P_AppId).FirstOrDefault();
        }

        public List<DB.Context.Theme> _Get_Themes(int P_AppId)
        {
            return G_DB_ThingyContext.Themes.Where(t => t.AppId == P_AppId).ToList();
        }

        public DB.Context.Theme _Get_Theme(int P_ThemeId)
        {
            return G_DB_ThingyContext.Themes.Where(t => t.Id == P_ThemeId).FirstOrDefault();
        }

        public List<DB.Context.Action> _Get_Actions(int P_AppId)
        {
            return G_DB_ThingyContext.Actions.Where(a => a.AppId == P_AppId).ToList();
        }

        public List<DB.Context.Screen> _Get_Screens(int P_AppId)
        {
            return G_DB_ThingyContext.Screens.Where(s => s.AppId == P_AppId).ToList();
        }

        public DB.Context.Screen _Get_Screen(int P_ScreenId)
        {
            return G_DB_ThingyContext.Screens.Where(s => s.Id == P_ScreenId).FirstOrDefault();
        }

        public List<DB.Context.Screen_Control> _Get_Screen_Controls(int P_ScreenId)
        {
            return G_DB_ThingyContext.Screen_Controls.Where(si => si.ScreenId == P_ScreenId).OrderBy(c => c.Position).ToList();
        }


        public List<DB.Context.Action> _Get_ControlActions(int P_ControlId)
        {
            return (from a in G_DB_ThingyContext.Actions
                       join ca in G_DB_ThingyContext.Control_Actions on a.Id equals ca.ActionId
                       where ca.ControlId == P_ControlId
                       select a).ToList();

            //return G_DB_ThingyContext.Actions.Where(a => a.Control_Actions.ScreenId == P_ScreenId).ToList();
        }

        public List<DB.Context.Control_Action> _Get_Control_Actions(int P_ControlId)
        {
            return G_DB_ThingyContext.Control_Actions.Where(a => a.ControlId == P_ControlId).OrderBy(c => c.Position).ToList();
        }

    }
}
