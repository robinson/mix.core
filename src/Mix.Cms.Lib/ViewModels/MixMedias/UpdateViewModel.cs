﻿using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixSystem;
using Mix.Common.Helper;
using Mix.Domain.Core.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixMedias
{
    public class UpdateViewModel
        : ViewModelBase<MixCmsContext, MixMedia, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please choose File")]
        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("fileFolder")]
        public string FileFolder { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("fileSize")]
        public int FileSize { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("targetUrl")]
        public string TargetUrl { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }
        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

        [JsonProperty("fullPath")]
        public string FullPath
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(TargetUrl))
                {
                    return FileFolder.IndexOf("http") > 0 ? $"{FileFolder}/{FileName}{Extension}"
                        : $"{Domain}/{FileFolder}/{FileName}{Extension}";
                }
                else
                {
                    return TargetUrl;
                }
            }
        }
        [JsonProperty("filePath")]
        public string FilePath
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(TargetUrl))
                {
                    return FileFolder.IndexOf("http") > 0 ? $"{FileFolder}/{FileName}{Extension}"
                        : $"/{FileFolder}/{FileName}{Extension}";
                }
                else
                {
                    return TargetUrl;
                }
            }
        }

        [JsonProperty("mediaFile")]
        public FileViewModel MediaFile { get; set; }
        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixMedia model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixMedia ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default(DateTime))
            {
                Id = Id > 0 ? Id : UpdateViewModel.Repository.Max(c => c.Id).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
                IsClone = true;
                Cultures = Cultures ?? LoadCultures(Specificulture, _context, _transaction);
                Cultures.ForEach(c => c.IsSupported = true);
            }
            if (string.IsNullOrEmpty(TargetUrl))
            {
                if (FileFolder[0] == '/') { FileFolder = FileFolder.Substring(1); }
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void Validate(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (MediaFile?.FileStream != null)
            {
                MediaFile.Filename = SeoHelper.GetSEOString(MediaFile.Filename).ToLower() + Guid.NewGuid().ToString("N");
                MediaFile.FileFolder = CommonHelper.GetFullPath(new[] {
                    //MixService.GetConfig<string>("UploadFolder"),
                    MixService.GetTemplateUploadFolder(Specificulture),
                    DateTime.UtcNow.ToString("yyyy-MM")
                }); ;
                var isSaved = FileRepository.Instance.SaveWebFile(MediaFile);
                if (isSaved)
                {
                    Extension = MediaFile.Extension.ToLower();
                    FileName = MediaFile.Filename;
                    FileFolder = MediaFile.FileFolder;
                }
                else
                {
                    IsValid = false;
                }

            }
            FileType = FileType ?? "image";
            base.Validate(_context, _transaction);

        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Cultures = LoadCultures(Specificulture, _context, _transaction);
            MediaFile = new FileViewModel();
        }

        public override RepositoryResponse<bool> RemoveRelatedModels(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>
            {
                IsSucceed = FileRepository.Instance.DeleteWebFile(FileName, Extension, FileFolder)
            };
            result.IsSucceed = Repository.RemoveListModel(false, m => m.Id == Id && m.Specificulture != Specificulture, _context, _transaction).IsSucceed;
            return result;
        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            // Remove local file
            if (FileFolder.IndexOf("http") < 0)
            {
                FileRepository.Instance.DeleteWebFile(FileName, Extension, FileFolder);
            }
            await Repository.RemoveListModelAsync(false, m => m.Id == Id && m.Specificulture != Specificulture, _context, _transaction);
            return await base.RemoveRelatedModelsAsync(view, _context, _transaction);
        }

        #endregion Overrides
        #region Expand
        List<SupportedCulture> LoadCultures(string initCulture = null, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getCultures = SystemCultureViewModel.Repository.GetModelList(_context, _transaction);
            var result = new List<SupportedCulture>();
            if (getCultures.IsSucceed)
            {
                foreach (var culture in getCultures.Data)
                {
                    result.Add(
                        new SupportedCulture()
                        {
                            Icon = culture.Icon,
                            Specificulture = culture.Specificulture,
                            Alias = culture.Alias,
                            FullName = culture.FullName,
                            Description = culture.FullName,
                            Id = culture.Id,
                            Lcid = culture.Lcid,
                            IsSupported = culture.Specificulture == initCulture || _context.MixMedia.Any(p => p.Id == Id && p.Specificulture == culture.Specificulture)
                        });

                }
            }
            return result;
        }
        #endregion

    }
}
