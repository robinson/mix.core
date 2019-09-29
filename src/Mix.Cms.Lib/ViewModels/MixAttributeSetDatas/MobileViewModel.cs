﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class MobileViewModel
      : ODataViewModelBase<MixCmsContext, MixAttributeSetData, MobileViewModel>
    {
        #region Properties
        #region Models
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }
        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        #endregion Models
        #region Views

        //public List<MixAttributeSetValues.MobileViewModel> Values { get; set; }
        public JObject Data { get; set; }
        #endregion
        #endregion Properties

        #region Contructors

        public MobileViewModel() : base()
        {
        }

        public MobileViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Data = new JObject();
            Data.Add(new JProperty("id", Id));
            var values = MixAttributeSetValues.MobileViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
            foreach (var item in values.OrderBy(v=>v.Priority))
            {
                Data.Add(ParseValue(item));
            }
        }

        //public override Task<RepositoryResponse<MobileViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        //{
        //    string dbName = $"data/attribute-sets/{AttributeSetName}.sqlite";
        //    var cnn = new SqliteConnection($"Data Source={dbName};Version=3;");
        //    // open the connection:
        //    //using (var connection = new SqliteConnection("Data Source=hello.db"))
        //    //{
        //    //    var command = connection.CreateCommand();
        //    //    command.CommandText =
        //    //        "UPDATE message SET text = $text1 WHERE id = 1;" +
        //    //        "UPDATE message SET text = $text2 WHERE id = 2";
        //    //    command.Parameters.AddWithValue("$text1", "Hello");
        //    //    command.Parameters.AddWithValue("$text2", "World");

        //    //    connection.Open();
        //    //    command.ExecuteNonQuery();
        //    //}

        //    //if (value != null)
        //    //{
        //    //    var jobj = JObject.FromObject(value);
        //    //    var cacheFile = new FileViewModel()
        //    //    {
        //    //        Filename = key.ToLower(),
        //    //        Extension = ".json",
        //    //        FileFolder = "Cache",
        //    //        Content = jobj.ToString(Newtonsoft.Json.Formatting.None)
        //    //    };

        //    //    var result = FileRepository.Instance.SaveFile(cacheFile);
        //    //    return new RepositoryResponse<bool>()
        //    //    {
        //    //        IsSucceed = result,
        //    //    };
        //    //}
        //    //else
        //    //{
        //    //    return new RepositoryResponse<bool>();
        //    //}
        //    var result = new RepositoryResponse<MobileViewModel>()
        //    {
        //        IsSucceed = true,
        //        Data = this
        //    };
        //    return Task.FromResult(result);
        //}

        #endregion

        #region Expands
        JProperty ParseValue(MixAttributeSetValues.MobileViewModel item)
        {
            switch (item.DataType)
            {
                case MixEnums.MixDataType.DateTime:
                    return new JProperty(item.AttributeFieldName, item.DateTimeValue);
                case MixEnums.MixDataType.Date:
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));
                case MixEnums.MixDataType.Time:
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));
                case MixEnums.MixDataType.Double:
                    return (new JProperty(item.AttributeFieldName, item.DoubleValue));
                case MixEnums.MixDataType.Boolean:
                    return (new JProperty(item.AttributeFieldName, item.BooleanValue));
                case MixEnums.MixDataType.Number:
                    return (new JProperty(item.AttributeFieldName, item.IntegerValue));
                case MixEnums.MixDataType.Reference:
                    string url = $"/api/v1/odata/en-us/related-attribute-set-data/mobile/parent/set/{Id}/{item.Field.ReferenceId}";
                    //foreach (var nav in item.DataNavs)
                    //{
                    //    arr.Add(nav.Data.Data);
                    //}
                    return (new JProperty(item.AttributeFieldName, url));
                case MixEnums.MixDataType.Custom:
                case MixEnums.MixDataType.Duration:
                case MixEnums.MixDataType.PhoneNumber:
                case MixEnums.MixDataType.Text:
                case MixEnums.MixDataType.Html:
                case MixEnums.MixDataType.MultilineText:
                case MixEnums.MixDataType.EmailAddress:
                case MixEnums.MixDataType.Password:
                case MixEnums.MixDataType.Url:
                case MixEnums.MixDataType.ImageUrl:
                case MixEnums.MixDataType.CreditCard:
                case MixEnums.MixDataType.PostalCode:
                case MixEnums.MixDataType.Upload:
                case MixEnums.MixDataType.Color:
                case MixEnums.MixDataType.Icon:
                case MixEnums.MixDataType.VideoYoutube:
                case MixEnums.MixDataType.TuiEditor:
                default:
                    return (new JProperty(item.AttributeFieldName, item.StringValue));
            }
        }
        #endregion
    }
}
