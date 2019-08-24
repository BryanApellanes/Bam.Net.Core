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
using Bam.Net.Services.DataReplication.Data;
using Bam.Net.Services.DataReplication.Data.Dao;

namespace Bam.Net.Services.DataReplication.Data.Wrappers
{
	// generated
	[Serializable]
	public class QueryOperationWrapper: Bam.Net.Services.DataReplication.Data.QueryOperation, IHasUpdatedXrefCollectionProperties
	{
		public QueryOperationWrapper()
		{
			this.UpdatedXrefCollectionProperties = new Dictionary<string, PropertyInfo>();
		}

		public QueryOperationWrapper(DaoRepository repository) : this()
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

        System.Collections.Generic.List<Bam.Net.Services.DataReplication.Data.DataPropertyFilter> _propertyFilters;
		public override System.Collections.Generic.List<Bam.Net.Services.DataReplication.Data.DataPropertyFilter> PropertyFilters
		{
			get
			{
				if (_propertyFilters == null)
				{
					_propertyFilters = DaoRepository.ForeignKeyCollectionLoader<Bam.Net.Services.DataReplication.Data.QueryOperation, Bam.Net.Services.DataReplication.Data.DataPropertyFilter>(this).ToList();
				}
				return _propertyFilters;
			}
			set
			{
				_propertyFilters = value;
			}
		}



	}
	// -- generated
}																								
