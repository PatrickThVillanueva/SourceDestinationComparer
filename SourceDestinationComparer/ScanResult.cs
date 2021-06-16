using System.Collections.Generic;

namespace SourceDestinationComparer
{
    public class ScanResult<TSource, TDestination>
    {
        public ScanResult()
        {
            this.NewItems = new List<ScanItem<TSource, TDestination>>();
            this.OrphanItems = new List<ScanItem<TSource, TDestination>>();
            this.UpdatedItems = new List<ScanItem<TSource, TDestination>>();
            this.IdenticalItems = new List<ScanItem<TSource, TDestination>>();
        }

        /// <summary>
        /// Items only in TSource, and not in TDestination
        /// </summary>
        public List<ScanItem<TSource, TDestination>> NewItems { get; private set; }

        /// <summary>
        /// Items in both TSource and TDestination, new data
        /// </summary>
        public List<ScanItem<TSource, TDestination>> UpdatedItems { get; private set; }

        /// <summary>
        /// Items in both TSource and TDestination, identical data
        /// </summary>
        public List<ScanItem<TSource, TDestination>> IdenticalItems { get; private set; }

        /// <summary>
        /// List of items in TDestination, but missing in TSource
        /// </summary>
        public List<ScanItem<TSource, TDestination>> OrphanItems { get; private set; }
    }
}
