namespace SearchAlgorithms
{
    public class LruCache<TKey, TValue> where TKey : notnull
    {
        // Dictionary voor snelle opzoeking via key
        private readonly Dictionary<TKey, LinkedListNode<(TKey key, TValue value)>> cache;

        // LinkedList houdt de volgorde bij: achteraan = recent gebruikt, vooraan = langst geleden
        private readonly LinkedList<(TKey key, TValue value)> order;

        // Maximum aantal items in de cache
        private readonly int maxSize;

        // Tellers om hits en misses bij te houden
        public int Hits { get; private set; }
        public int Misses { get; private set; }

        public LruCache(int maxSize)
        {
            this.maxSize = maxSize;
            cache = new Dictionary<TKey, LinkedListNode<(TKey, TValue)>>();
            order = new LinkedList<(TKey, TValue)>();
            Hits = 0;
            Misses = 0;
        }

        // Probeer een waarde op te halen uit de cache
        // Geeft true terug als het gevonden is (hit), false als het er niet in zit (miss)
        public bool TryGet(TKey key, out TValue value)
        {
            if (cache.TryGetValue(key, out var node))
            {
                // HIT! Verplaats naar achteraan (= recent gebruikt)
                order.Remove(node);
                order.AddLast(node);
                value = node.Value.value;
                Hits++;
                return true;
            }

            // MISS! Zit niet in de cache
            value = default!;
            Misses++;
            return false;
        }

        // Sla een resultaat op in de cache
        public void Put(TKey key, TValue value)
        {
            // Als de key al bestaat, update de waarde en verplaats naar achteraan
            if (cache.TryGetValue(key, out var existingNode))
            {
                order.Remove(existingNode);
                existingNode.Value = (key, value);
                order.AddLast(existingNode);
                return;
            }

            // Cache vol? Gooi het VOORSTE element eruit (= langst niet meer gebruikt, LRU!)
            if (cache.Count >= maxSize)
            {
                var oldest = order.First!;
                cache.Remove(oldest.Value.key);
                order.RemoveFirst();
            }

            // Voeg het nieuwe element toe achteraan (= meest recent)
            var newNode = order.AddLast((key, value));
            cache[key] = newNode;
        }

        // Reset de cache en tellers
        public void Clear()
        {
            cache.Clear();
            order.Clear();
            Hits = 0;
            Misses = 0;
        }
    }
}