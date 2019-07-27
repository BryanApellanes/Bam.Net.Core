# Raft Consensus

RaftRing - The top level local state tracking component; holds references to all nodes participating in the raft consensus protocol.

RaftNode - Represents a single node in the raft consensus protocol.  RaftRing holds references to RaftNodes by way of the Arcs defined on the ring.  RaftNodes communicate through the RaftRing by providing to the RaftRing a RaftClient configured for communication to its RaftRing.
