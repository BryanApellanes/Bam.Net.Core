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
	public class HeaderDataWrapper: Bam.Net.Logging.Http.Data.HeaderData, IHasUpdatedXrefCollectionProperties
	{
		public HeaderDataWrapper()
		{
			this.UpdatedXrefCollectionProperties = new Dictionary<string, PropertyInfo>();
		}

		public HeaderDataWrapper(DaoRepository repository) : this()
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


        Bam.Net.Logging.Http.Data.ResponseData _responseData;
		public override Bam.Net.Logging.Http.Data.ResponseData ResponseData
		{
			get
			{
				if (_responseData == null)
				{
					_responseData = (Bam.Net.Logging.Http.Data.ResponseData)DaoRepository.GetParentPropertyOfChild(this, typeof(Bam.Net.Logging.Http.Data.ResponseData));
				}
				return _responseData;
			}
			set
			{
				_responseData = value;
			}
		}        Bam.Net.Logging.Http.Data.RequestData _requestData;
		public override Bam.Net.Logging.Http.Data.RequestData RequestData
		{
			get
			{
				if (_requestData == null)
				{
					_requestData = (Bam.Net.Logging.Http.Data.RequestData)DaoRepository.GetParentPropertyOfChild(this, typeof(Bam.Net.Logging.Http.Data.RequestData));
				}
				return _requestData;
			}
			set
			{
				_requestData = value;
			}
		}


	}
	// -- generated
}																								
