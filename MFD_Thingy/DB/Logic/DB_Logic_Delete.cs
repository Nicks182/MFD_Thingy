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
        public void _Delete_App(Application P_App)
        {

            List<DB.Context.Screen> L_Screens = _Get_Screens(P_App.Id);

            List<DB.Context.Screen_Control> L_ScreenControls = null;
            List<DB.Context.Control_Action> L_ControlActions = null;
            foreach (var L_Screen in L_Screens)
            {
                L_ScreenControls = _Get_Screen_Controls(L_Screen.Id);
                
                foreach(var L_Control in L_ScreenControls)
                {
                    L_ControlActions = _Get_Control_Actions(L_Control.Id);

                    G_DB_ThingyContext.Control_Actions.RemoveRange(L_ControlActions);
                }

                G_DB_ThingyContext.Screen_Controls.RemoveRange(L_ScreenControls);
            }

            G_DB_ThingyContext.Screens.RemoveRange(L_Screens);


            List<DB.Context.Action> L_Actions = _Get_Actions(P_App.Id);
            G_DB_ThingyContext.Actions.RemoveRange(L_Actions);

            List<DB.Context.Theme> L_Themes = _Get_Themes(P_App.Id);
            G_DB_ThingyContext.Themes.RemoveRange(L_Themes);

            G_DB_ThingyContext.Applications.Remove(P_App);
            G_DB_ThingyContext.SaveChanges();

        }

        public void _Delete_Theme(Theme P_Theme)
        {
            G_DB_ThingyContext.Themes.Remove(P_Theme);
            G_DB_ThingyContext.SaveChanges();
        }

        public void _Delete_Action(DB.Context.Action P_Action)
        {
            List<DB.Context.Control_Action> L_ControlActions = G_DB_ThingyContext.Control_Actions.Where(ca => ca.ActionId == P_Action.Id).ToList();
            G_DB_ThingyContext.Control_Actions.RemoveRange(L_ControlActions);

            G_DB_ThingyContext.Actions.Remove(P_Action);
            G_DB_ThingyContext.SaveChanges();
        }

        public void _Delete_Screen(Screen P_Screen)
        {
            List<DB.Context.Screen_Control> L_ScreenControls = null;
            List<DB.Context.Control_Action> L_ControlActions = null;

            L_ScreenControls = _Get_Screen_Controls(P_Screen.Id);

            foreach (var L_Control in L_ScreenControls)
            {
                L_ControlActions = _Get_Control_Actions(L_Control.Id);

                G_DB_ThingyContext.Control_Actions.RemoveRange(L_ControlActions);
            }

            G_DB_ThingyContext.Screen_Controls.RemoveRange(L_ScreenControls);

            G_DB_ThingyContext.Screens.Remove(P_Screen);
            G_DB_ThingyContext.SaveChanges();
        }

        public void _Delete_Control(Screen_Control P_Control)
        {
            List<DB.Context.Control_Action> L_ControlActions = G_DB_ThingyContext.Control_Actions.Where(ca => ca.ControlId == P_Control.Id).ToList();
            G_DB_ThingyContext.Control_Actions.RemoveRange(L_ControlActions);

            G_DB_ThingyContext.Screen_Controls.Remove(P_Control);
            G_DB_ThingyContext.SaveChanges();
        }

        public void _Delete_Control_Action(Control_Action P_Control_Action)
        {
            G_DB_ThingyContext.Control_Actions.Remove(P_Control_Action);
            G_DB_ThingyContext.SaveChanges();
        }
    }
}
