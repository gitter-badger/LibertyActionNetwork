using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Li.Lan.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int batchSize)
        {
            return items
                .Select((item, inx) => new { item, inx })
                .GroupBy(x => x.inx / batchSize)
                .Select(g => g.Select(x => x.item));
        }

        /// <summary>
        /// Performs a Fisher-Yates shuffle on the List http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
        /// Simulates drawing cards randomly out of a hat.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void ShuffleInPlace<T>(this IList<T> list)
        {
            var random = CreateCryptoRandom();

            // Start with the current index being the list.Count
            // We will reduce this number each time we "pull" items out of the collection and sort them to the "top"
            int currentIndex = list.Count;

            while (currentIndex > 1)
            {
                // get a random index that is lower than the previously "pulled"/"shuffled" items
                int randomIndex = random.Next(currentIndex);
                currentIndex--;

                // get reference to whatever is at the currentIndex (in order to "swap" it with whatever is at the random draw index)
                T temp = list[currentIndex];

                // take the item at the randomIndex (random draw out of a hat) and place it at the current index (aka sort it to the "top")
                list[currentIndex] = list[randomIndex];

                // move the item that was at the currentIndex, to the place that was holding the now "pulled"/"shuffled" item (aka perform the "swap")
                list[randomIndex] = temp;
            }
        }

        /// <summary>
        /// Performs a Fisher-Yates shuffle on the List http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
        /// Simulates drawing cards randomly out of a hat.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list"></param>
        public static List<TSource> Shuffle<TSource>(this List<TSource> list)
        {
            var result = list.ToList();

            var random = CreateCryptoRandom();

            // Start with the current index being the list.Count
            // We will reduce this number each time we "pull" items out of the collection and sort them to the "top"
            int currentIndex = result.Count;

            while (currentIndex > 1)
            {
                // get a random index that is lower than the previously "pulled"/"shuffled" items
                int randomIndex = random.Next(currentIndex);
                currentIndex--;

                // get reference to whatever is at the currentIndex (in order to "swap" it with whatever is at the random draw index)
                TSource temp = result[currentIndex];

                // take the item at the randomIndex (random draw out of a hat) and place it at the current index (aka sort it to the "top")
                result[currentIndex] = result[randomIndex];

                // move the item that was at the currentIndex, to the place that was holding the now "pulled"/"shuffled" item (aka perform the "swap")
                result[randomIndex] = temp;
            }

            return result;
        }

        /// <summary>
        /// Create a random number generator using a Crypto random number seed
        /// </summary>
        /// <returns></returns>
        private static Random CreateCryptoRandom()
        {
            return new Random(GenerateRandomInt32());
        }

        private static int GenerateRandomInt32()
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[sizeof(int)];
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}