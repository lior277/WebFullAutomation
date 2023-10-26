using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Graphics.Operations.SpecialGraphicsState;

namespace AirSoftAutomationFramework.Internals.ExtensionsMethods
{
    public static class JObjectExtensions
    {
        public static IEnumerable<JProperty> RemovePropertiesFromObject(this JObject actualJObject,
            string[] propertiesToRemove)
        {
            foreach (var item in propertiesToRemove)
            {
                actualJObject.Property(item).Remove();
            }

            return actualJObject.Properties();
        }

        public static bool DeepCompare(this object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            //Compare two object's class, return false if they are difference
            if (obj.GetType() != another.GetType()) return false;

            var result = true;
            //Get all properties of obj
            //And compare each other
            foreach (var property in obj.GetType().GetProperties())
            {
                var objValue = property.GetValue(obj);
                var anotherValue = property.GetValue(another);
                if (!objValue.Equals(anotherValue)) result = false;
            }

            return result;
        }

        public static List<string> CompareTwoObjects(this object obj,
            object another, List<string> tableColumnNames)
        {
            var diff = new Dictionary<string, object>(); 
            
            foreach (var columnName in tableColumnNames)
            {
                var parentPropValue = obj
                    .GetType()
                    .GetProperties()?
                    .Where(p => p.Name == columnName)?
                    .FirstOrDefault()?
                    .GetValue(obj)?
                    .ToString();

                var childPropValue = another
                    .GetType()
                    .GetProperties()?
                    .Where(p => p.Name == columnName)?
                    .FirstOrDefault()?
                    .GetValue(another)?
                    .ToString();

                if (parentPropValue != childPropValue)
                {
                    diff.Add(columnName, new Dictionary<string, object> { { childPropValue, parentPropValue } });
                }
            }

            return diff.Keys.ToList();
        }
    }
}
