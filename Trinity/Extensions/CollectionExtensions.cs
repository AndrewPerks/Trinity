using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Trinity.Extensions
{
    /// <summary>
    /// Converts various collections to another type of collection
    /// </summary>
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }
    }
}
