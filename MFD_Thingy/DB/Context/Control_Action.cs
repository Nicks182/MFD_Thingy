﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MFD_Thingy.DB.Context
{
    public partial class Control_Action
    {
        public int Id { get; set; }
        public int? ControlId { get; set; }
        public int? ActionId { get; set; }
        public int? Position { get; set; }

        public virtual Action Action { get; set; }
        public virtual Screen_Control Control { get; set; }
    }
}