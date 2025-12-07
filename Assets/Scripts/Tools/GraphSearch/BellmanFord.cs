using System.Collections.Generic;

namespace Tools.GraphSearch
{
    public class BellmanFord : IGraphSearch
    {
        public List<int> GetPath(
            Dictionary<int, Node<Coord>> adjacencyList,
            int source,
            int destination)
        {
            int verticesCount = adjacencyList.Count;
            int[] predecessor = new int[verticesCount];

            if (BreadthFirstSearch(adjacencyList, source, destination, verticesCount, predecessor) == false)
            {
                return null;
            }

            List<int> path = new List<int>();
            int crawl = destination;
            path.Add(crawl);

            while (predecessor[crawl] != -1)
            {
                path.Add(predecessor[crawl]);
                crawl = predecessor[crawl];
            }

            return path;
        }

        private bool BreadthFirstSearch(
            Dictionary<int, Node<Coord>> adjacencyList,
            int source,
            int destination,
            int verticesCount,
            int[] predecessor)
        {
            Queue<int> verticesToCheck = new Queue<int>();
            bool[] visited = new bool[verticesCount];

            for (int i = 0; i < verticesCount; i++)
            {
                visited[i] = false;
                predecessor[i] = -1;
            }

            visited[source] = true;
            verticesToCheck.Enqueue(source);

            while (verticesToCheck.Count != 0)
            {
                int vertexToCheck = verticesToCheck.Dequeue();
                HashSet<int> neighbours = adjacencyList[vertexToCheck].GetAdjacentVertices();

                foreach (var neighbour in neighbours)
                {
                    if (visited[neighbour] == false)
                    {
                        visited[neighbour] = true;
                        predecessor[neighbour] = vertexToCheck;
                        verticesToCheck.Enqueue(neighbour);

                        if (neighbour == destination)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}