﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Mix.Identity.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;


namespace Mix.Cms.Web.Controllers
{
    public class PostController : BaseController
    {        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        IApplicationLifetime _lifetime;

        #region contructor
        public PostController(IHostingEnvironment env,
            IMemoryCache memoryCache,
             UserManager<ApplicationUser> userManager,
             IApiDescriptionGroupCollectionProvider apiExplorer,
            IHttpContextAccessor accessor,
            IApplicationLifetime lifetime
            ) : base(env, memoryCache, accessor)
        {

            this._userManager = userManager;
            _apiExplorer = apiExplorer;
            _lifetime = lifetime;
        }

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (MixService.GetConfig<bool>("IsInit"))
            {
                isValid = false;
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = $"Init";
                }
                else
                {
                    var status = MixService.GetConfig<string>("InitStatus");
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }
        #endregion

        #region Routes

        [Route("post/{id}/{seoName}")]
        [Route("post/{culture}/{id}/{seoName}")]
        public async Task<IActionResult> Index(int id, string culture, string seoName)
        {
            if (isValid)
            {
                return await Post(id);
            }
            else
            {
                return Redirect(_redirectUrl);
            }            
        }

        #endregion

    }
}