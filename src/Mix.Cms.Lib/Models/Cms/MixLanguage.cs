﻿using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixLanguage
    {
        public string Keyword { get; set; }
        public string Specificulture { get; set; }
        public string Category { get; set; }
        public int DataType { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }

        public MixCulture SpecificultureNavigation { get; set; }
    }
}
