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
	public class RaftVoteWrapper: Bam.Net.Services.DataReplication.Consensus.Data.RaftVote, IHasUpdatedXrefCollectionProperties
	{
		public RaftVoteWrapper()
		{
			this.UpdatedXrefCollectionProperties = new Dictionary<string, PropertyInfo>();
		}

		public RaftVoteWrapper(DaoRepository repository) : this()
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


        Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection _raftLeaderElection;
		public override Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection RaftLeaderElection
		{
			get
			{
				if (_raftLeaderElection == null)
				{
					_raftLeaderElection = (Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection)DaoRepository.GetParentPropertyOfChild(this, typeof(Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection));
				}
				return _raftLeaderElection;
			}
			set
			{
				_raftLeaderElection = value;
			}
		}


	}
	// -- generated
}																								
