using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceDestinationComparer
{
    public class SourceDestinationComparer
    {
        /// <summary>
        /// Iterates through TSource and TDestination, and returns a ScanResult of new items, updated items and orphaned items using TKey
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="sourceKey"></param>
        /// <param name="destination"></param>
        /// <param name="destinationKey"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        public static ScanResult<TSource, TDestination> Scan<TSource, TDestination, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> sourceKey, IEnumerable<TDestination> destination, Func<TDestination, TKey> destinationKey, Func<TSource, TDestination, bool> equalityComparer)
            where TKey : IComparable
        {
            var ScanResult = new ScanResult<TSource, TDestination>();
            var sourceCount = source.Count();
            var destinationCount = destination.Count();
            if (sourceCount == 0 && destinationCount == 0)
            {
                // Do nothing
            }
            else if (sourceCount > 0 && destinationCount == 0)
            {
                ScanResult.NewItems.AddRange(source.Select(x => new ScanItem<TSource, TDestination>() { Source = x }));
            }
            else if (sourceCount == 0 && destinationCount > 0)
            {
                ScanResult.OrphanItems.AddRange(destination.Select(x => new ScanItem<TSource, TDestination>() { Destination = x }));
            }
            else
            {
                var orderedSource = source.OrderBy(sourceKey);
                var orderedDestination = destination.OrderBy(destinationKey);

                var sourceIterator = orderedSource.GetEnumerator();
                var destinationIterator = orderedDestination.GetEnumerator();

                var sourceHasNext = sourceIterator.MoveNext() && sourceIterator.Current != null;
                var destinationHasNext = destinationIterator.MoveNext() && destinationIterator.Current != null;

                while (sourceHasNext && destinationHasNext)
                {
                    var sourceItem = sourceIterator.Current;
                    var destinationItem = destinationIterator.Current;
                    var compareResult = sourceKey(sourceItem).CompareTo(destinationKey(destinationItem));
                    if (compareResult == 0)
                    {
                        if (equalityComparer(sourceItem, destinationItem))
                        {
                            ScanResult.IdenticalItems.Add(new ScanItem<TSource, TDestination>() { Destination = destinationItem, Source = sourceItem });
                        }
                        else
                        {
                            ScanResult.UpdatedItems.Add(new ScanItem<TSource, TDestination>() { Destination = destinationItem, Source = sourceItem });
                        }

                        sourceHasNext = sourceIterator.MoveNext();
                        destinationHasNext = destinationIterator.MoveNext();
                    }
                    else if (compareResult < 0)
                    {
                        ScanResult.NewItems.Add(new ScanItem<TSource, TDestination>() { Source = sourceItem });
                        sourceHasNext = sourceIterator.MoveNext();
                    }
                    else
                    {
                        ScanResult.OrphanItems.Add(new ScanItem<TSource, TDestination>() { Destination = destinationItem });
                        destinationHasNext = destinationIterator.MoveNext();
                    }
                }

                while (sourceHasNext)
                {
                    ScanResult.NewItems.Add(new ScanItem<TSource, TDestination>() { Source = sourceIterator.Current });
                    sourceHasNext = sourceIterator.MoveNext();
                }

                while (destinationHasNext)
                {
                    ScanResult.OrphanItems.Add(new ScanItem<TSource, TDestination>() { Destination = destinationIterator.Current });
                    destinationHasNext = destinationIterator.MoveNext();
                }
            }

            return ScanResult;
        }
    }
}
