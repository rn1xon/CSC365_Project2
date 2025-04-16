using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC365_Project2.BplusOps
{
    public class Node<TKey, TValue>
    {
        public bool IsLeaf { get; set; }
        public List<TKey> Keys { get; set; } = [];
        public List<Node<TKey, TValue>> Children { get; set; } = [];
        public List<TValue> Values { get; set; } = [];
        public Node<TKey, TValue> Next { get; set; }
    }
}
