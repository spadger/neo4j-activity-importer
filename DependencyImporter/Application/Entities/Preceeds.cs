using Neo4jClient;

namespace DependencyImporter.Application.Entities
{
    public class Preceeds : Relationship, IRelationshipAllowingSourceNode<Activity>, IRelationshipAllowingTargetNode<Activity>
    {
        public Preceeds(NodeReference targetNode) : base(targetNode)
        {
        }

        public override string RelationshipTypeKey => "Preceeds";
    }
}