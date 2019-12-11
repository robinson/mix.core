﻿// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/file")]
    public class ApiFileController : BaseApiController<MixCmsContext>
    {
        public ApiFileController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }
        #region Post

        // Post api/files/id
        [HttpGet, HttpOptions]
        [Route("details")]
        public RepositoryResponse<FileViewModel> Details(string folder, string filename)
        {
            // Request: Key => folder, Keyword => filename
            if (!string.IsNullOrEmpty(folder))
            {
                var file = FileRepository.Instance.GetFile(filename, folder);

                return new RepositoryResponse<FileViewModel>()
                {
                    IsSucceed = file != null,
                    Data = file
                };
            }
            else
            {
                return new RepositoryResponse<FileViewModel>();
            }
        }

        // GET api/files/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public RepositoryResponse<bool> Delete(RequestObject request)
        {
            string fullPath = CommonHelper.GetFullPath(new string[]
            {
                request.Key,
                request.Keyword
            });
            var result = FileRepository.Instance.DeleteWebFile(fullPath);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = result,
                Data = result
            };
        }

        // POST api/values
        /// <summary>
        /// Uploads the image.
        /// </summary>
        /// <param name="image">The img information.</param>
        /// <param name="file"></param> Ex: { "base64": "", "fileFolder":"" }
        /// <returns></returns>
        [Route("uploadFile")]
        [HttpPost, HttpOptions]
        public IActionResult Edit(FileViewModel file)
        {
            if (ModelState.IsValid)
            {
                var result = FileRepository.Instance.SaveWebFile(file);
                return Ok(result);
            }
            return BadRequest();
        }

        // POST api/files
        [HttpPost, HttpOptions]
        [Route("save")]
        public RepositoryResponse<FileViewModel> Save([FromBody]FileViewModel model)
        {
            if (model != null)
            {
                var result = FileRepository.Instance.SaveFile(model);
                return new RepositoryResponse<FileViewModel>()
                {
                    IsSucceed = result,
                    Data = model
                };
            }
            return new RepositoryResponse<FileViewModel>();
        }

        // GET api/files

        [HttpPost, HttpOptions]
        [Route("list")]
        public RepositoryResponse<FilePageViewModel> GetList([FromBody]RequestPaging request)
        {
            ParseRequestPagingDate(request);
            var files = FileRepository.Instance.GetTopFiles(request.Key);
            var directories = FileRepository.Instance.GetTopDirectories(request.Key);
            return new RepositoryResponse<FilePageViewModel>()
            {
                IsSucceed = true,
                Data = new FilePageViewModel()
                {
                    Files = files,
                    Directories = directories
                }
            };
        }
        #endregion Post


    }
}
