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
using Bam.Net.Services.DataReplication.Consensus.Data;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Wrappers
{
	// generated
	[Serializable]
	public class RaftLeaderElectionWrapper: Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection, IHasUpdatedXrefCollectionProperties
	{
		public RaftLeaderElectionWrapper()
		{
			this.UpdatedXrefCollectionProperties = new Dictionary<string, PropertyInfo>();
		}

		public RaftLeaderElectionWrapper(DaoRepository repository) : this()
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

        System.Collections.Generic.List<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote> _votes;
		public override System.Collections.Generic.List<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote> Votes
		{
			get
			{
				if (_votes == null)
				{
					_votes = DaoRepository.ForeignKeyCollectionLoader<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection, Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>(this).ToList();
				}
				return _votes;
			}
			set
			{
				_votes = value;
			}
		}



	}
	// -- generated
}																								
