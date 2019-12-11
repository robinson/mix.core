﻿using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPositions
{
    public class UpdateViewModel
        : ViewModelBase<MixCmsContext, MixPosition, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("pages")]
        public List<MixPagePositions.UpdateViewModel> Pages { get; set; } = new List<MixPagePositions.UpdateViewModel>();
        #endregion
        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixPosition model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixPosition ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(p => p.Id).Data + 1;
            }
            return base.ParseModel(_context, _transaction);
        }
        #region Async
        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixPosition parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            //Save current set attribute
            foreach (var page in Pages)
            {
                //Save current set attribute
                var saveResult = await page.SaveModelAsync(false, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            //Save current set attribute
            foreach (var page in Pages)
            {
                //Save current set attribute
                var removeResult = await page.RemoveModelAsync(false, _context, _transaction);
                ViewModelHelper.HandleResult(removeResult, ref result);
            }
            return result;
        }
        #endregion

        #endregion

        #region Expands

        public async Task LoadPageAsync(string culture)
        {
            var getPages = await MixPagePositions.UpdateViewModel.Repository.GetModelListByAsync(
                    p => p.Specificulture == culture && p.PositionId == Id);
            Pages = getPages.Data;
        }

        #endregion
    }
}
