using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DependencyImporter.Application.Entities;
using DependencyImporter.Application.Storage;
using Neo4jClient;

namespace DependencyImporter.Application
{
    public class Importer
    {
        private readonly IProvideStorage _storageProvider;

        public Importer(IProvideStorage storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public void Import(IEnumerable<Activity> activityStream, string[] edgeDefninitions, IProgress<ImportProgress> progress, CancellationToken cancellationToken)
        {
            var activities = activityStream.ToDictionary(MakeKey);

            var importProgress = new ImportProgress(activities.Count, edgeDefninitions.Length);
            
            var nodeReferences = new Dictionary<string, NodeReference<Activity>>();

            var i = 0;

            foreach (var activity in activities.Values)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var nodeReference = _storageProvider.Create(activity);
                nodeReferences.Add(MakeKey(activity), nodeReference);

                if (++i % 20 == 0)
                {
                    importProgress.CompletedItems = i;
                    progress.Report(importProgress);
                }
            }

            foreach (var edgeDefinition in edgeDefninitions)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var values = edgeDefinition.Split(',').Select(x => x.Trim()).ToArray();
                var from = nodeReferences[MakeKey(values[0], values[1])];
                var to = nodeReferences[MakeKey(values[2], values[3])];

                var relationshipType = values[4];
                var freeFloat = decimal.Parse(values[5]);
                var driving = values[6]=="Yes";
                var critical = values[7]=="Yes";
                var lag = string.IsNullOrEmpty(values[8])?0: int.Parse(values[8]);

                var payload = new PreceedsPayload(relationshipType, freeFloat, driving, critical, lag);

                _storageProvider.CreateRelationship(from, new Preceeds(to, payload));

                if (++i%20 == 0)
                {
                    importProgress.CompletedItems = i;
                    progress.Report(importProgress);
                }
            }

            importProgress.CompletedItems = i;
            progress.Report(importProgress);
        }

        public async Task DeleteAll()
        {
            await _storageProvider.DeleteAllAsync();
        }

        public static string MakeKey(Activity activity)
        {
            return MakeKey(activity.ProjectId, activity.ActivityId);
        }

        public static string MakeKey(string projectId, string activityId)
        {
            return $"{projectId}|{activityId}".ToUpper();
        }
    }
}
