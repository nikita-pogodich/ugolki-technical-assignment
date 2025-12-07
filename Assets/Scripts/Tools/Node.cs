using System;
using System.Collections.Generic;

namespace Tools
{
    public class Node<T>
    {
        private readonly int _id;
        private readonly HashSet<int> _adjacencySet;
        private readonly T _value;

        public T Value => _value;
        public int Id => _id;

        public Node(int id, T value)
        {
            _id = id;
            _value = value;
            _adjacencySet = new HashSet<int>();
        }

        public void AddEdge(int nodeId)
        {
            if (_id == nodeId)
            {
                throw new ArgumentException("The vertex cannot be adjacent to itself");
            }

            _adjacencySet.Add(nodeId);
        }

        public HashSet<int> GetAdjacentVertices()
        {
            return _adjacencySet;
        }

        public override bool Equals(object obj)
        {
            return _value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}