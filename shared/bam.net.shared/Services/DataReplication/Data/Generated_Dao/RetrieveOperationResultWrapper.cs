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
	public class RetrieveOperationResultWrapper: Bam.Net.Services.DataReplication.Data.RetrieveOperationResult, IHasUpdatedXrefCollectionProperties
	{
		public RetrieveOperationResultWrapper()
		{
			this.UpdatedXrefCollectionProperties = new Dictionary<string, PropertyInfo>();
		}

		public RetrieveOperationResultWrapper(DaoRepository repository) : this()
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





	}
	// -- generated
}																								
