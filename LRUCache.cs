using System.Collections.Generic;

namespace LRUCache
{
    public class LRUCache<TKey, TValue>
    {
        private int cacheSize;

        private Dictionary<TKey, TValue> valueByKey = new Dictionary<TKey, TValue>();

        private LinkedList<TKey> list = new LinkedList<TKey>();

        private Dictionary<TKey, LinkedListNode<TKey>> nodeByKey = new Dictionary<TKey, LinkedListNode<TKey>>();

        public LRUCache(int cacheSize)
        {
            // if the cache size is less then one
            if (cacheSize < 1)
            {
                // then throw an exception
                throw new InvalidCacheSizeException(string.Format("Cache size {0} is invalid", cacheSize));
            }

            this.cacheSize = cacheSize;
        }

        /// <summary>
        /// Adds the provided value to the cache with the provided key.
        /// The time complexity of this operation is O(1).
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            // if the cache already contains the key
            if (valueByKey.ContainsKey(key))
            {
                // then update the corresponding value
                valueByKey[key] = value;

                // and set the key to be the most recently used one
                var node = nodeByKey[key];
                list.Remove(node);
                list.AddFirst(node);
            }
            else
            {
                // else, put the key and the corresponding value to the cache
                valueByKey.Add(key, value);

                // create the list node
                var newNode = new LinkedListNode<TKey>(key);

                // add the list node to the front of the list 
                // (which means that the corresponding key has been used most recently)
                list.AddFirst(newNode);

                // add the list node to the hashtable, to ensure O(1) access
                nodeByKey.Add(key, newNode);

                // if the cache size became too large
                if (list.Count > cacheSize)
                {
                    // remove the node from the nodeByKey hashtable
                    nodeByKey.Remove(list.Last.Value);

                    // remove the corresponding entry from the cache
                    valueByKey.Remove(list.Last.Value);

                    // then remove the least recently used element from the list
                    list.RemoveLast();
                }
            }
        }

        /// <summary>
        /// Checks if the entry with the provided key is present in the cache.
        /// If it is present, then the method sets it to be the most recently used key and return the corresponding value.
        /// Else, the method return the default value of type TValue.
        /// The time complexity of this operation is O(1).
        /// </summary>
        public TValue Get(TKey key)
        {
            // if the cache contains the key
            if (valueByKey.ContainsKey(key))
            {
                // then move the list node corresponding to the requested key to the front of the list
                // (which means that the corresponding key has been used most recently)
                var node = nodeByKey[key];
                list.Remove(node);
                list.AddFirst(node);

                // remove the cached value
                return valueByKey[key];
            }
            else
            {
                // else throw the exception
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Checks if the cache contains the provided key.
        /// The time complexity of this operation is O(1).
        /// </summary>
        public bool Contains(TKey key)
        {
            return valueByKey.ContainsKey(key);
        }
    }
}
