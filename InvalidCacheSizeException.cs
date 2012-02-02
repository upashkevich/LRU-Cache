using System;

namespace LRUCache
{
    /// <summary>
    /// This is an exception which is thrown when the LRU cache is constructed with non-positive cache size
    /// </summary>
    public class InvalidCacheSizeException : Exception
    {
        public InvalidCacheSizeException(string message) : base(message)
        {
        }
    }
}
