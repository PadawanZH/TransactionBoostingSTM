using System;
using System.Collections.Concurrent;

namespace TransactionBoosting.Test
{
    public class TestConcurrentDictionary
    {
        private ConcurrentDictionary<int, int> _map;

        public TestConcurrentDictionary()
        {
            _map = new ConcurrentDictionary<int, int>();
        }

        public void testMap()
        {
            for (int i = 0; i < 10; i++)
            {
                _map[i] = i * i;
            }

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("({0}, {1}) should be {2}", i, _map[i], i*i);
            }

            for (int i = 1; i < 3; i++)
            {
                _map.AddOrUpdate(i, i * i, (k, v) => v * 3);
                Console.WriteLine("({0}, {1}) should be {2}", i, _map[i], i*i);
            }

            for (int i = 1; i < 30; i+=3)
            {
                _map.GetOrAdd(i, (k) =>
                {
                    Console.WriteLine("When key {0}, called the defined add function", k);
                    return 0;
                });
                
                Console.WriteLine("After try insert into map, ({0}, {1})", i, _map[i]);
            }
            
        }
    }
}