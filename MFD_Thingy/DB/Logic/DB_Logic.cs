using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MFD_Thingy.DB.Context;

namespace MFD_Thingy
{
    public partial class DB_Logic
    {
        DB_ThingyContext G_DB_ThingyContext = null;

        public DB_Logic()
        {
            G_DB_ThingyContext = new DB_ThingyContext();
        }

        

    }
}
