using System.Threading;
using System.Threading.Tasks;
using Neo4jClient;

namespace DependencyImporter.Application.Storage
{
    public class FakeStorageProvider : IProvideStorage
    {
        private static long count = 0;
        public void Dispose(){}

        public RelationshipReference CreateRelationship<TSourceNode, TRelationship>(NodeReference<TSourceNode> sourceNodeReference, TRelationship relationship) where TRelationship : Relationship, IRelationshipAllowingSourceNode<TSourceNode>
        {
            var value = Interlocked.Increment(ref count);
            return new RelationshipReference(value);
        }

        public NodeReference<TNode> Create<TNode>(TNode node, params IRelationshipAllowingParticipantNode<TNode>[] relationships) where TNode : class
        {
            var value = Interlocked.Increment(ref count);
            return new NodeReference<TNode>(value);
        }

        public Task DeleteAllAsync()
        {
            return Task.FromResult(0);
        }
    }
}