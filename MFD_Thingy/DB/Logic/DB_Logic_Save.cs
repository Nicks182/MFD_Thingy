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
        public void _Save_App(Application P_App)
        {
            if(P_App.Id == 0)
            {
                G_DB_ThingyContext.Applications.Add(P_App);
            }

            G_DB_ThingyContext.SaveChanges();

        }

        public void _Save_Theme(Theme P_Theme)
        {
            if (P_Theme.Id == 0)
            {
                G_DB_ThingyContext.Themes.Add(P_Theme);
            }

            G_DB_ThingyContext.SaveChanges();
        }

        public void _Save_Action(DB.Context.Action P_Action)
        {
            if (P_Action.Id == 0)
            {
                G_DB_ThingyContext.Actions.Add(P_Action);
            }

            G_DB_ThingyContext.SaveChanges();
        }

        public void _Save_Screen(Screen P_Screen)
        {
            if (P_Screen.Id == 0)
            {
                G_DB_ThingyContext.Screens.Add(P_Screen);
            }

            G_DB_ThingyContext.SaveChanges();
        }

        public void _Save_Control(Screen_Control P_Control)
        {
            if (P_Control.Id == 0)
            {
                G_DB_ThingyContext.Screen_Controls.Add(P_Control);
            }

            G_DB_ThingyContext.SaveChanges();
        }

        public void _Save_Control_Action(Control_Action P_Control_Action)
        {
            if (P_Control_Action.Id == 0)
            {
                G_DB_ThingyContext.Control_Actions.Add(P_Control_Action);
            }

            G_DB_ThingyContext.SaveChanges();
        }
    }
}
