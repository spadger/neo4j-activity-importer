namespace DependencyImporter.Application
{
    public class ImportProgress
    {
        private readonly int _totalNodes;
        private readonly int _totalEdges;
        private readonly int _totalThings;
        
        public ImportProgress(int totalNodes, int totalEdges)
        {
            _totalNodes = totalNodes;
            _totalEdges = totalEdges;

            _totalThings = totalNodes +_totalEdges;
        }

        public int CompletedItems { private get; set; }

        public float TotalPercentage => ((float) CompletedItems/(float)_totalThings) *100f;

        public string Message
        {
            get
            {
                if (CompletedItems <= _totalNodes)
                {
                    return $"Importing node {CompletedItems} of {_totalNodes}";
                }
                else
                {
                    return $"Importing relationship {CompletedItems-_totalNodes} of {_totalEdges}";
                }
            }
        }


    }
}