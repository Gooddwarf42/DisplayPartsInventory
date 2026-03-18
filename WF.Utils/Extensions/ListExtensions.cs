namespace WF.Utils.Extensions;

// TODO make these methods more in general for enumerables
public static class ListExtensions
{
    // TODO: add an optional comparer
    public static void AddIfNotPresent<T>(this List<T> source, T item)
    {
        if (source.Contains(item))
        {
            return;
        }

        source.Add(item);
    }

    // TODO: add an optional comparer
    public static void AddWithoutDuplicates<T>(this List<T> source, IEnumerable<T> itemsToAdd)
    {
        source.AddRange(itemsToAdd.Where(item => source.All(sourceItem => !sourceItem!.Equals(item))));
    }
}