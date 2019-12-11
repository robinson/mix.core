﻿using System;
using System.Collections.Generic;

namespace Mix.Cms.Web.Models.Cms
{
    public partial class MixPortalPage
    {
        public MixPortalPage()
        {
            MixPortalPageNavigationIdNavigation = new HashSet<MixPortalPageNavigation>();
            MixPortalPageNavigationParent = new HashSet<MixPortalPageNavigation>();
            MixPortalPagePosition = new HashSet<MixPortalPagePosition>();
            MixPortalPageRole = new HashSet<MixPortalPageRole>();
        }

        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public string Icon { get; set; }
        public string TextKeyword { get; set; }
        public int Status { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string TextDefault { get; set; }
        public int Level { get; set; }

        public virtual ICollection<MixPortalPageNavigation> MixPortalPageNavigationIdNavigation { get; set; }
        public virtual ICollection<MixPortalPageNavigation> MixPortalPageNavigationParent { get; set; }
        public virtual ICollection<MixPortalPagePosition> MixPortalPagePosition { get; set; }
        public virtual ICollection<MixPortalPageRole> MixPortalPageRole { get; set; }
    }
}
