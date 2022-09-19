﻿using System;
using System.Collections.Generic;

namespace InfraestructureLayer.DataModels
{
    /// <summary>
    /// Product names and descriptions. Product descriptions are provided in multiple languages.
    /// </summary>
    public partial class VProductAndDescription
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string ProductModel { get; set; } = null!;
        public string Culture { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
