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
using Bam.Net.CoreServices.WebHooks.Data;
using Bam.Net.CoreServices.WebHooks.Data.Dao;

namespace Bam.Net.CoreServices.WebHooks.Data.Wrappers
{
	// generated
	[Serializable]
	public class WebHookCallWrapper: Bam.Net.CoreServices.WebHooks.Data.WebHookCall, IHasUpdatedXrefCollectionProperties
	{
		public WebHookCallWrapper()
		{
			this.UpdatedXrefCollectionProperties = new Dictionary<string, PropertyInfo>();
		}

		public WebHookCallWrapper(DaoRepository repository) : this()
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


        Bam.Net.CoreServices.WebHooks.Data.WebHookDescriptor _webHookDescriptor;
		public override Bam.Net.CoreServices.WebHooks.Data.WebHookDescriptor WebHookDescriptor
		{
			get
			{
				if (_webHookDescriptor == null)
				{
					_webHookDescriptor = (Bam.Net.CoreServices.WebHooks.Data.WebHookDescriptor)DaoRepository.GetParentPropertyOfChild(this, typeof(Bam.Net.CoreServices.WebHooks.Data.WebHookDescriptor));
				}
				return _webHookDescriptor;
			}
			set
			{
				_webHookDescriptor = value;
			}
		}


	}
	// -- generated
}																								
