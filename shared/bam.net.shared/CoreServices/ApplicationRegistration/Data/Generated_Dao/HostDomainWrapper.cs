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
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.CoreServices.ApplicationRegistration.Data.Dao;

namespace Bam.Net.CoreServices.ApplicationRegistration.Data.Wrappers
{
	// generated
	[Serializable]
	public class HostDomainWrapper: Bam.Net.CoreServices.ApplicationRegistration.Data.HostDomain, IHasUpdatedXrefCollectionProperties
	{
		public HostDomainWrapper()
		{
			this.UpdatedXrefCollectionProperties = new Dictionary<string, PropertyInfo>();
		}

		public HostDomainWrapper(DaoRepository repository) : this()
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




        // right xref

// Right Xref property: Left -> Application ; Right -> HostDomain
		List<Bam.Net.CoreServices.ApplicationRegistration.Data.Application> _applications;
		public override List<Bam.Net.CoreServices.ApplicationRegistration.Data.Application> Applications
		{
			get
			{
				if(_applications == null || _applications.Count == 0)
				{
					var xref = new XrefDaoCollection<Bam.Net.CoreServices.ApplicationRegistration.Data.Dao.ApplicationHostDomain, Bam.Net.CoreServices.ApplicationRegistration.Data.Dao.Application>(DaoRepository.GetDaoInstance(this), false);
					xref.Load(DaoRepository.Database);
					_applications = ((IEnumerable)xref).CopyAs<Bam.Net.CoreServices.ApplicationRegistration.Data.Application>().ToList();
					SetUpdatedXrefCollectionProperty("Applications", this.GetType().GetProperty("Applications"));					
				}

				return _applications;
			}
			set
			{
				_applications = value;
				SetUpdatedXrefCollectionProperty("Applications", this.GetType().GetProperty("Applications"));
			}
		}

	}
	// -- generated
}																								
