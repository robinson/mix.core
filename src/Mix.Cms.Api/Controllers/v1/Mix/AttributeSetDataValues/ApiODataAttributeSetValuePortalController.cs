﻿// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixAttributeSetValues;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Mix.Cms.Api.Controllers.v1.AttributeSetValues
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/attribute-set-value")]
    public class ApiAttributeSetValueController :
        BaseGenericApiController<MixCmsContext, MixAttributeSetValue>
    {
        public ApiAttributeSetValueController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        // GET api/attribute-set-value/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixAttributeSetValue>> DeleteAsync(string id)
        {
            return await base.DeleteAsync<DeleteViewModel>(model => model.Id == id, true);
        }

        // GET api/attribute-set-values/id
        [HttpGet, HttpOptions]
        [Route("details/{id}/{viewType}")]
        [Route("details/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, string id)
        {
            string msg = string.Empty;
            switch (viewType)
            {
                case "portal":
                    if (!string.IsNullOrEmpty(id))
                    {
                        Expression<Func<MixAttributeSetValue, bool>> predicate = model => model.Id == id;
                        var portalResult = await base.GetSingleAsync<MobileViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixAttributeSetValue()
                        {
                            Status = MixService.GetConfig<int>("DefaultStatus")
                            ,
                            Priority = MobileViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<MobileViewModel> result = await base.GetSingleAsync<MobileViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (!string.IsNullOrEmpty(id))
                    {
                        Expression<Func<MixAttributeSetValue, bool>> predicate = model => model.Id == id;
                        var result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(result));
                    }
                    else
                    {
                        var model = new MixAttributeSetValue()
                        {
                            Status = MixService.GetConfig<int>("DefaultStatus")
                            ,
                            Priority = ReadViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<ReadViewModel> result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
            }
        }



        #region Post

        // POST api/attribute-set-value
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<MobileViewModel>> Save([FromBody]MobileViewModel data)
        {
            if (data != null)
            {
                data.Specificulture = _lang;
                var result = await base.SaveAsync<MobileViewModel>(data, true);
                if (result.IsSucceed)
                {
                    MixService.LoadFromDatabase();
                    MixService.SaveSettings();
                }
                return result;
            }
            return new RepositoryResponse<MobileViewModel>() { Status = 501 };
        }

        // GET api/attribute-set-value
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var parsed = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isLevel = int.TryParse(parsed.Get("level"), out int level);
            ParseRequestPagingDate(request);
            Expression<Func<MixAttributeSetValue, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.StringValue.Contains(request.Keyword))
                            )
                        && (!request.FromDate.HasValue
                            || (model.CreatedDateTime >= request.FromDate.Value)
                        )
                        && (!request.ToDate.HasValue
                            || (model.CreatedDateTime <= request.ToDate.Value)
                        );
            string key = $"{request.Key}_{request.PageSize}_{request.PageIndex}";
            switch (request.Key)
            {
                case "mvc":
                    var mvcResult = await base.GetListAsync<ReadMvcViewModel>(key, request, predicate);                    
                    return Ok(JObject.FromObject(mvcResult));
                default:

                    var listItemResult = await base.GetListAsync<ReadViewModel>(key, request, predicate);                    
                    return JObject.FromObject(listItemResult);
            }
        }
        #endregion Post
    }
}