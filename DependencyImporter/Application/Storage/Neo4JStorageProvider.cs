using System;
using System.Linq;
using System.Threading.Tasks;
using DependencyImporter.Application.Entities;
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

        public NodeReference<Activity> Create(Activity activity)
        {
            var node =_graphClient.Cypher
                .Create($"(act:{activity.Type.Replace(' ', '_')} {{act}})")
                .WithParam("act", activity)
                .Return(act => act.Node<Activity>())
                .Results
                .Single();

            return node.Reference;
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