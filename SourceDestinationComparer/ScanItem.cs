namespace SourceDestinationComparer
{
    public class ScanItem<TSource, TDestination>
    {
        public TSource Source { get; set; }

        public TDestination Destination { get; set; }
    }
}
