using Neo4jClient;

namespace DependencyImporter.Application.Entities
{
    public class Preceeds : Relationship, IRelationshipAllowingSourceNode<Activity>, IRelationshipAllowingTargetNode<Activity>
    {
        public Preceeds(NodeReference targetNode, string relationshipType, decimal freeFloat, bool driving, bool critical, int lag) : base(targetNode)
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

        public override string RelationshipTypeKey => "Preceeds";
    }
}