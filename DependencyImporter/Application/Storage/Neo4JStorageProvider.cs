using System;
using System.Threading.Tasks;
using Neo4jClient;

namespace DependencyImporter.Application.Storage
{
    public class Neo4JStorageProvider : IProvideStorage
    {
        readonly GraphClient _graphClient;
        
        public Neo4JStorageProvider(string uri, string username, string password)
        {
            _graphClient = new GraphClient(new Uri(uri), username, password);
            _graphClient.Connect();
        }

        public void Dispose()
        {
            _graphClient.Dispose();
        }

        public RelationshipReference CreateRelationship<TSourceNode, TRelationship>(NodeReference<TSourceNode> sourceNodeReference, TRelationship relationship) where TRelationship : Relationship, IRelationshipAllowingSourceNode<TSourceNode>
        {
            return _graphClient.CreateRelationship(sourceNodeReference, relationship);
        }

        public NodeReference<TNode> Create<TNode>(TNode node, params IRelationshipAllowingParticipantNode<TNode>[] relationships) where TNode : class
        {
            return _graphClient.Create(node, relationships);
        }

        public async Task DeleteAllAsync()
        {
            await _graphClient.Cypher
                                .Match("(n)")
                                .DetachDelete("(n)")
                                .ExecuteWithoutResultsAsync()
                                .ConfigureAwait(false);
        }
    }
}