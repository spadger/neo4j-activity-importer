using System;
using System.Threading.Tasks;
using Neo4jClient;

namespace DependencyImporter.Application.Storage
{
    public interface IProvideStorage: IDisposable
    {
        RelationshipReference CreateRelationship<TSourceNode, TRelationship>(NodeReference<TSourceNode> sourceNodeReference, TRelationship relationship) where TRelationship : Relationship, IRelationshipAllowingSourceNode<TSourceNode>;
        NodeReference<TNode> Create<TNode>(TNode node, params IRelationshipAllowingParticipantNode<TNode>[] relationships) where TNode : class;
        Task DeleteAllAsync();
    }
}