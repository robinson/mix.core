﻿using System;
using System.Collections.Generic;

namespace Mix.Cms.Web.Models.Cms
{
    public partial class MixModulePost
    {
        public int PostId { get; set; }
        public int ModuleId { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixModule MixModule { get; set; }
        public virtual MixPost MixPost { get; set; }
    }
}
