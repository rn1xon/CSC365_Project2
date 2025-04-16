using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSC365_Project2.BplusOps
{
    public class Tree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private readonly int _maxDegree;
        private Node<TKey, TValue> _root;

        public Tree(int maxDegree)
        {
            if (maxDegree < 3)
            {
                throw new ArgumentOutOfRangeException("maxDegree must be at least 3");
            }
            _maxDegree = maxDegree;
            _root = new Node<TKey, TValue>();
            _root.IsLeaf = true;
        }

        /// <summary>
        /// Inserts a new node into the tree
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Insert(TKey key, TValue value)
        {
            var leaf = FindLeafSpot(key);
            InsertInLeaf(leaf, key, value);

            if (leaf.Keys.Count == _maxDegree)
            {
                SplitNode(leaf);
            }
        }

        /// <summary>
        /// Traverses the tree to find a location for a new node based on its key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Node<TKey, TValue> FindLeafSpot(TKey key) 
        {
            var current = _root;

            while (!current.IsLeaf)
            {
                int i = current.Keys.FindIndex(k => k.CompareTo(key) > 0); // Searches for the position to insert this item based on the key passed in
                if (i == -1)
                {
                    current = current.Children[^1]; // Last item of the children
                }
                else
                {
                    current = current.Children[i];
                }
            }
            return current;
        }

        /// <summary>
        /// Inserts a new node at the right place
        /// </summary>
        /// <param name="leaf"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void InsertInLeaf(Node<TKey, TValue> leaf, TKey key, TValue value)
        {
            int index = leaf.Keys.BinarySearch(key);
            if (index >= 0)
            {
                throw new InvalidOperationException("Cannot insert a duplicate item");
            }
            leaf.Keys.Insert(~index, key);
            leaf.Values.Insert(~index, value);
        }

        /// <summary>
        /// Rearranges tree when max degree is reached
        /// </summary>
        /// <param name="node"></param>
        private void SplitNode(Node<TKey, TValue> node)
        {
            int medianIndex = node.Keys.Count/2;
            TKey medianKey = node.Keys[medianIndex];

            // create the right side node 
            Node<TKey, TValue> rightNode = new()
            {
                IsLeaf = node.IsLeaf, 
                Keys = node.Keys.GetRange(medianIndex + 1, node.Keys.Count - medianIndex - 1), // the keys after the median are moved to the right node

                //assigns child pointers to the Children property of the new rightNode, but only if the node is an internal node(!node.IsLeaf).
                Children = node.IsLeaf ? null : node.Children.GetRange(medianIndex + 1, node.Children.Count - medianIndex - 1),

                // If the node is a leaf, the corresponding values associated with the moved keys are also transferred to the rightNode.
                // If the node is an internal node, Values is irrelevant (set to null) because internal nodes store child pointers instead of actual values.
                Values = node.IsLeaf ? node.Values.GetRange(medianIndex + 1, node.Values.Count - medianIndex -1) : null
            };

            // adjust the left node (this is the current node)
            node.Keys.RemoveRange(medianIndex, node.Keys.Count - medianIndex); // Removing all the keys that we moved to the new rightside node
            if (!node.IsLeaf)
            {
                // for internal nodes 
                node.Children.RemoveRange(medianIndex + 1, node.Children.Count - medianIndex - 1); // move the child pointers for the keys we moved from the rightside
            }
            if (node.IsLeaf)
            {
                // for leaf nodes 
                if(node.Children?.Count > 0)
                {
                    node.Children.RemoveRange(medianIndex, node.Values.Count - medianIndex); //removes the values associated with the keys that we moved to the rightside
                }
            }

            // update leaf node links 
            if (node.IsLeaf)
            {
                rightNode.Next = node.Next;
                node.Next = rightNode;
            }

            // Promote the median key to the parent
            if (node == _root)
            {
                // Create a new root if splitting the root
                _root = new Node<TKey, TValue>
                {
                    Keys = new List<TKey> { medianKey },
                    Children = new List<Node<TKey, TValue>> { node, rightNode },
                    IsLeaf = false,
                };
            }
            else
            {
                var parent = FindParent(_root, node); // Helper method to find the parent node
                int insertIndex = parent.Keys.BinarySearch(medianKey);
                if (insertIndex < 0) insertIndex = ~insertIndex;

                parent.Keys.Insert(insertIndex, medianKey);
                parent.Children.Insert(insertIndex + 1, rightNode);

                // Recursively split the parent if needed
                if (parent.Keys.Count == _maxDegree)
                    SplitNode(parent);
            }

        }

        private Node<TKey, TValue> FindParent(Node<TKey, TValue> currentNode, Node<TKey, TValue> childNode)
        {
            // Base case: If the current node is a leaf, it has no children, so no parent exists
            if (currentNode.IsLeaf || currentNode.Children == null)
                return null;

            // Check if any child of the current node is the target child node
            foreach (var child in currentNode.Children)
            {
                if (child == childNode)
                {
                    return currentNode;
                }
            }

            // Recursively search through the children
            foreach (var child in currentNode.Children)
            {
                var parent = FindParent(child, childNode);
                if (parent != null)
                {
                    return parent;
                }
            }

            // If no parent is found (edge case), return null
            return null;
        }

        public TValue Search(TKey key)
        {
            var current = _root;
            while (!current.IsLeaf)
            {
                int i = current.Keys.FindIndex(k => k.CompareTo(key) > 0);
                current = i == -1 ? current.Children[^1] : current.Children[i];
            }

            int index = current.Keys.IndexOf(key);
            return index != -1 ? current.Values[index] : default;
        }

        public void TraverseLeaves()
        {
            // Start at the root and follow the leftmost child to reach the first leaf
            var current = _root;
            while (!current.IsLeaf)
            {
                current = current.Children[0]; // Follow the leftmost child
            }

            // Traverse through the leaf nodes using the Next pointer
            while (current != null)
            {
                foreach (var key in current.Keys)
                {
                    Console.Write($"{key} "); // Print the keys in sorted order
                }
                current = current.Next; // Move to the next leaf node
            }
            Console.WriteLine(); // End of traversal
        }


        /// <summary>
        /// Creates Tree visualization 
        /// </summary>
        /// <param name="fileName"></param>
        public void WriteTreeToFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            File.AppendAllText(fileName, DisplayNode(_root, 0, ""));
        }

        private string DisplayNode(Node<TKey, TValue> node, int level, string prefix)
        {
            StringBuilder sb = new StringBuilder();

            // Indentation with branching
            sb.AppendLine($"{prefix}├── {string.Join(", ", node.Keys)}");

            if (!node.IsLeaf)
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    bool isLastChild = (i == node.Children.Count - 1);
                    string childPrefix = prefix + (isLastChild ? "    " : "│   ");
                    sb.Append(DisplayNode(node.Children[i], level + 1, childPrefix));
                }
            }

            return sb.ToString();
        }

    }

}
