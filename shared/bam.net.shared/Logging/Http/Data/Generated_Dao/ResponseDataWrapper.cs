using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Linq;
using Bam.Net;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;
using Newtonsoft.Json;
using Bam.Net.Logging.Http.Data;
using Bam.Net.Logging.Http.Data.Dao;

namespace Bam.Net.Logging.Http.Data.Wrappers
{
	// generated
	[Serializable]
	public class ResponseDataWrapper: Bam.Net.Logging.Http.Data.ResponseData, IHasUpdatedXrefCollectionProperties
	{
		public ResponseDataWrapper()
		{
			this.UpdatedXrefCollectionProperties = new Dictionary<string, PropertyInfo>();
		}

		public ResponseDataWrapper(DaoRepository repository) : this()
		{
			this.DaoRepository = repository;
		}

		[JsonIgnore]
		public DaoRepository DaoRepository { get; set; }

		[JsonIgnore]
		public Dictionary<string, PropertyInfo> UpdatedXrefCollectionProperties { get; set; }

		protected void SetUpdatedXrefCollectionProperty(string propertyName, PropertyInfo correspondingProperty)
		{
			if(UpdatedXrefCollectionProperties != null && !UpdatedXrefCollectionProperties.ContainsKey(propertyName))
			{
				UpdatedXrefCollectionProperties.Add(propertyName, correspondingProperty);				
			}
			else if(UpdatedXrefCollectionProperties != null)
			{
				UpdatedXrefCollectionProperties[propertyName] = correspondingProperty;				
			}
		}

        System.Collections.Generic.List<Bam.Net.Logging.Http.Data.CookieData> _cookies;
		public override System.Collections.Generic.List<Bam.Net.Logging.Http.Data.CookieData> Cookies
		{
			get
			{
				if (_cookies == null)
				{
					_cookies = DaoRepository.ForeignKeyCollectionLoader<Bam.Net.Logging.Http.Data.ResponseData, Bam.Net.Logging.Http.Data.CookieData>(this).ToList();
				}
				return _cookies;
			}
			set
			{
				_cookies = value;
			}
		}        System.Collections.Generic.List<Bam.Net.Logging.Http.Data.HeaderData> _headers;
		public override System.Collections.Generic.List<Bam.Net.Logging.Http.Data.HeaderData> Headers
		{
			get
			{
				if (_headers == null)
				{
					_headers = DaoRepository.ForeignKeyCollectionLoader<Bam.Net.Logging.Http.Data.ResponseData, Bam.Net.Logging.Http.Data.HeaderData>(this).ToList();
				}
				return _headers;
			}
			set
			{
				_headers = value;
			}
		}



	}
	// -- generated
}																								
