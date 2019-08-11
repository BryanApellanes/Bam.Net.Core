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
	public class DataPointOriginWrapper: Bam.Net.Services.DataReplication.Data.DataPointOrigin, IHasUpdatedXrefCollectionProperties
	{
		public DataPointOriginWrapper()
		{
			this.UpdatedXrefCollectionProperties = new Dictionary<string, PropertyInfo>();
		}

		public DataPointOriginWrapper(DaoRepository repository) : this()
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

        System.Collections.Generic.List<Bam.Net.Services.DataReplication.Data.DataPoint> _dataPoints;
		public override System.Collections.Generic.List<Bam.Net.Services.DataReplication.Data.DataPoint> DataPoints
		{
			get
			{
				if (_dataPoints == null)
				{
					_dataPoints = DaoRepository.ForeignKeyCollectionLoader<Bam.Net.Services.DataReplication.Data.DataPointOrigin, Bam.Net.Services.DataReplication.Data.DataPoint>(this).ToList();
				}
				return _dataPoints;
			}
			set
			{
				_dataPoints = value;
			}
		}



	}
	// -- generated
}																								
