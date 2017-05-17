﻿using System;

namespace FerryConverter.Config
{
    [Flags]
    public enum StationCategory
    {
        None = 0,
        Modern = 1,
        Old = 2,
        Tram = 4,
        All = Modern | Old | Tram
    }
}
