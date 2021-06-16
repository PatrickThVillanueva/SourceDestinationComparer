# SourceDestinationComparer
Compares sourceData with destinationData, and returns a list of new, updated and missing items.

## Usage
This is a static class, which takes: 
IEnumerable source
a comparable key of the source
IEnumerable destination
a comparable key of the destination
a comparer of the source and destination

With these parameters, the algorithm will run through both the source and destination and calculate (through the keys), 
which items are missing at the destination, as well as finding out which items were updated (through the comparison of the data)
