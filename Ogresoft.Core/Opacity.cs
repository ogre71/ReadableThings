﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ogresoft
{
    /// <summary>
    /// These describe if an object can be looked into or out of. 
    /// </summary>
    [Flags]
    public enum Opacity
    {
        None = 0,
        In = 1,
        Out = 2
    };
}