﻿using System;
using System.Collections.Generic;

namespace Mix.Cms.Web.Models.Cms
{
    public partial class MixPageAttributeSet
    {
        public int PageId { get; set; }
        public string Specificulture { get; set; }
        public int AttributeSetId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixAttributeSet AttributeSet { get; set; }
        public virtual MixPage MixPage { get; set; }
    }
}
