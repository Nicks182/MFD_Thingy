﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MFD_Thingy.DB.Context
{
    public partial class Application
    {
        public Application()
        {
            Actions = new HashSet<Action>();
            Screens = new HashSet<Screen>();
            Themes = new HashSet<Theme>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageId { get; set; }

        public virtual ICollection<Action> Actions { get; set; }
        public virtual ICollection<Screen> Screens { get; set; }
        public virtual ICollection<Theme> Themes { get; set; }
    }
}