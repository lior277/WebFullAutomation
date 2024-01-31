using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirSoftAutomationFramework.Internals.ExtensionsMethods
{
    public static class IEnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> data, Action<T> action)
        {
            foreach (var d in data)
            {
                action(d);
            }
        }

        public static StringBuilder ListToString<T>(this IEnumerable<T> list)
        {
            var orderedList = list.OrderBy(q => q).ToList();
            var biulder = new StringBuilder();
            foreach (var item in orderedList)
            {
                biulder.Append(item).Append(", ");
                //biulder.Append(" " + "," + " ");
                biulder.AppendLine();
            }

            return biulder;
        }

        public static StringBuilder DictionaryToString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var orderedKeys = dictionary.Keys.OrderBy(q => q).ToList();
            var orderedValuess = dictionary.Values.OrderBy(q => q).ToList();
            var biulder = new StringBuilder();

            for (var i = 0; i < orderedKeys.Count; i++)
            {
                biulder.Append(orderedKeys[i]).Append(", ").Append(orderedValuess[i]);
                biulder.AppendLine();
            }               

            return biulder;
        }

        public static List<string> UpdateList(this List<string> actualList,
           Dictionary<List<string>, string> items)
        {
            if (items != null)
            {
                if (items.Values.First() == "remove")
                {
                    foreach (var item in items.Keys.First())
                    {
                        actualList.Remove(item);
                    }
                }
                if (items.Values.First() == "add")
                {
                    actualList.AddRange(items.Keys.First());
                }
            }

            return actualList;
        }

        public static List<string> GetDuplicatesFromTowListOfString(this IEnumerable<string> expectedList,
            IEnumerable<string> actualList)
        {
            var resultList = new List<string>();
            var duplicatesFromList1 = new List<string>();
            var duplicatesFromList2 = new List<string>();

            if (actualList.Count() != expectedList.Count())
            {
                duplicatesFromList1 = actualList.GroupBy(x => x)
                    .SelectMany(g => g.Skip(1))
                    .ToList();

                duplicatesFromList2 = expectedList.GroupBy(x => x)
                    .SelectMany(g => g.Skip(1))
                    .ToList();
            }

            resultList.AddRange(duplicatesFromList1);
            resultList.AddRange(duplicatesFromList2);

            return resultList;
        }

        public static List<string> CompareTwoListOfString(this List<string> actualList,
            List<string> expectedList)
        {
            var tempList = new List<string>();
            var orderActualList = actualList.OrderBy(x => x).ToList();
            var orderExpectedList = expectedList.OrderBy(x => x).ToList();
            var expectedExceptActual = expectedList.Except(orderActualList).ToList();
            var actualExceptExpected = actualList.Except(orderExpectedList).ToList();

            if (expectedExceptActual.Count != actualExceptExpected.Count)
            {
                foreach (var item in expectedList)
                {
                    if (!tempList.Contains(item) && expectedList.Where(p => p == item)
                            .Count() != actualList.Where(p => p == item).Count())
                    {
                        tempList.Add(item);
                    }
                }

                tempList.AddRange(actualExceptExpected);
            }
            else
            {
                foreach (var item in actualList)
                {
                    if (!tempList.Contains(item) && expectedList.Where(p => p == item)
                            .Count() != actualList.Where(p => p == item).Count())
                    {
                        tempList.Add(item);
                    }
                }

                tempList.AddRange(expectedExceptActual);
            }

            return tempList;
        }
    
        public static List<double> CompareListOfDouble(this IEnumerable<double> list1,
            IEnumerable<double> list2)
        {
            return list1.Except(list2).Union(list2.Except(list1)).ToList();
        }

        public static List<int> CompareListOfInt(this IEnumerable<int> expectedList,
            IEnumerable<int> actualList)
        {
            return expectedList.Except(actualList).Union(actualList.Except(expectedList)).ToList();
        }

        public static List<bool> CompareListOfBool(this IEnumerable<bool> list1, IEnumerable<bool> list2)
        {
            return list1.Except(list2).Union(list2.Except(list1)).ToList();
        }
    }
}
