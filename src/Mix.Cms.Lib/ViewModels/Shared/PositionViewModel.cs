﻿using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.Shared
{
    public class PositionViewModel
        : ViewModelBase<MixCmsContext, MixPosition, PositionViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public PositionViewModel() : base()
        {
        }

        public PositionViewModel(MixPosition model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
    }
}
