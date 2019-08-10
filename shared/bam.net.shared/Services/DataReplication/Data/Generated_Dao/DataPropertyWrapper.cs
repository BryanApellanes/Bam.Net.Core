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
	public class DataPropertyWrapper: Bam.Net.Services.DataReplication.Data.DataProperty, IHasUpdatedXrefCollectionProperties
	{
		public DataPropertyWrapper()
		{
			this.UpdatedXrefCollectionProperties = new Dictionary<string, PropertyInfo>();
		}

		public DataPropertyWrapper(DaoRepository repository) : this()
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


        Bam.Net.Services.DataReplication.Data.CreateOperation _createOperation;
		public override Bam.Net.Services.DataReplication.Data.CreateOperation CreateOperation
		{
			get
			{
				if (_createOperation == null)
				{
					_createOperation = (Bam.Net.Services.DataReplication.Data.CreateOperation)DaoRepository.GetParentPropertyOfChild(this, typeof(Bam.Net.Services.DataReplication.Data.CreateOperation));
				}
				return _createOperation;
			}
			set
			{
				_createOperation = value;
			}
		}        Bam.Net.Services.DataReplication.Data.DeleteEvent _deleteEvent;
		public override Bam.Net.Services.DataReplication.Data.DeleteEvent DeleteEvent
		{
			get
			{
				if (_deleteEvent == null)
				{
					_deleteEvent = (Bam.Net.Services.DataReplication.Data.DeleteEvent)DaoRepository.GetParentPropertyOfChild(this, typeof(Bam.Net.Services.DataReplication.Data.DeleteEvent));
				}
				return _deleteEvent;
			}
			set
			{
				_deleteEvent = value;
			}
		}        Bam.Net.Services.DataReplication.Data.DeleteOperation _deleteOperation;
		public override Bam.Net.Services.DataReplication.Data.DeleteOperation DeleteOperation
		{
			get
			{
				if (_deleteOperation == null)
				{
					_deleteOperation = (Bam.Net.Services.DataReplication.Data.DeleteOperation)DaoRepository.GetParentPropertyOfChild(this, typeof(Bam.Net.Services.DataReplication.Data.DeleteOperation));
				}
				return _deleteOperation;
			}
			set
			{
				_deleteOperation = value;
			}
		}        Bam.Net.Services.DataReplication.Data.SaveOperation _saveOperation;
		public override Bam.Net.Services.DataReplication.Data.SaveOperation SaveOperation
		{
			get
			{
				if (_saveOperation == null)
				{
					_saveOperation = (Bam.Net.Services.DataReplication.Data.SaveOperation)DaoRepository.GetParentPropertyOfChild(this, typeof(Bam.Net.Services.DataReplication.Data.SaveOperation));
				}
				return _saveOperation;
			}
			set
			{
				_saveOperation = value;
			}
		}        Bam.Net.Services.DataReplication.Data.UpdateOperation _updateOperation;
		public override Bam.Net.Services.DataReplication.Data.UpdateOperation UpdateOperation
		{
			get
			{
				if (_updateOperation == null)
				{
					_updateOperation = (Bam.Net.Services.DataReplication.Data.UpdateOperation)DaoRepository.GetParentPropertyOfChild(this, typeof(Bam.Net.Services.DataReplication.Data.UpdateOperation));
				}
				return _updateOperation;
			}
			set
			{
				_updateOperation = value;
			}
		}        Bam.Net.Services.DataReplication.Data.WriteEvent _writeEvent;
		public override Bam.Net.Services.DataReplication.Data.WriteEvent WriteEvent
		{
			get
			{
				if (_writeEvent == null)
				{
					_writeEvent = (Bam.Net.Services.DataReplication.Data.WriteEvent)DaoRepository.GetParentPropertyOfChild(this, typeof(Bam.Net.Services.DataReplication.Data.WriteEvent));
				}
				return _writeEvent;
			}
			set
			{
				_writeEvent = value;
			}
		}


	}
	// -- generated
}																								
