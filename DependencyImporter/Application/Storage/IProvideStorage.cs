using System;
using System.Threading.Tasks;
using DependencyImporter.Application.Entities;
using Neo4jClient;

namespace DependencyImporter.Application.Storage
{
    public interface IProvideStorage: IDisposable
    {
        RelationshipReference CreateRelationship<TSourceNode, TRelationship>(NodeReference<TSourceNode> sourceNodeReference, TRelationship relationship) where TRelationship : Relationship, IRelationshipAllowingSourceNode<TSourceNode>;
        NodeReference<Activity> Create(Activity activity);
        Task DeleteAllAsync();
    }
}