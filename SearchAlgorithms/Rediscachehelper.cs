using StackExchange.Redis;

namespace SearchAlgorithms
{
    public class RedisCacheHelper
    {
        // Connectie naar de Redis server
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase db;

        // Tellers om hits en misses bij te houden
        public int Hits { get; private set; }
        public int Misses { get; private set; }

        public RedisCacheHelper(string connectionString = "localhost:6379")
        {
            redis = ConnectionMultiplexer.Connect(connectionString);
            db = redis.GetDatabase();
            Hits = 0;
            Misses = 0;
        }

        // Probeer een waarde op te halen uit Redis
        // Zelfde logica als LruCache maar dan via de Redis server
        public bool TryGet(string key, out int value)
        {
            var result = db.StringGet(key);

            if (result.HasValue)
            {
                // HIT! Redis had het antwoord al
                value = (int)result;
                Hits++;
                return true;
            }

            // MISS! Niet in Redis, moet opnieuw berekend worden
            value = default;
            Misses++;
            return false;
        }

        // Sla een resultaat op in Redis
        // expiry is optioneel: hoe lang het resultaat bewaard blijft (TTL)
        public void Put(string key, int value, TimeSpan? expiry = null)
        {
            if (expiry.HasValue)
            {
                db.StringSet(key, value, new Expiration(expiry.Value));
            }
            else
            {
                db.StringSet(key, value);
            }
        }

        // Verwijder alle keys die met een bepaald prefix beginnen
        public void Clear()
        {
            var server = redis.GetServer(redis.GetEndPoints()[0]);
            foreach (var key in server.Keys(pattern: "search:*"))
            {
                db.KeyDelete(key);
            }
            Hits = 0;
            Misses = 0;
        }

        // Sluit de connectie
        public void Dispose()
        {
            redis.Dispose();
        }
    }
}