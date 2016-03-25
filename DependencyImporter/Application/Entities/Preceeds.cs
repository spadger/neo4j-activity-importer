using Neo4jClient;

namespace DependencyImporter.Application.Entities
{
    public class Preceeds : Relationship<PreceedsPayload>, IRelationshipAllowingSourceNode<Activity>, IRelationshipAllowingTargetNode<Activity>
    {

        public Preceeds(NodeReference targetNode, PreceedsPayload payload) : base(targetNode, payload)
        {}

        public override string RelationshipTypeKey => "Preceeds";
    }

    public class PreceedsPayload
    {
        public PreceedsPayload() { }

        public PreceedsPayload(string relationshipType, decimal freeFloat, bool driving, bool critical, int lag)
        {
            RelationshipType = relationshipType;
            FreeFloat = freeFloat;
            Driving = driving;
            Critical = critical;
            Lag = lag;
        }

        public string RelationshipType { get; set; }
        public decimal FreeFloat { get; set; }
        public bool Driving { get; set; }
        public bool Critical { get; set; }
        public int Lag { get; set; }
    }
}