﻿using System;
using System.Collections.Generic;

namespace Mix.Cms.Web.Models.Cms
{
    public partial class MixPosition
    {
        public MixPosition()
        {
            MixPagePosition = new HashSet<MixPagePosition>();
            MixPortalPagePosition = new HashSet<MixPortalPagePosition>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual ICollection<MixPagePosition> MixPagePosition { get; set; }
        public virtual ICollection<MixPortalPagePosition> MixPortalPagePosition { get; set; }
    }
}
