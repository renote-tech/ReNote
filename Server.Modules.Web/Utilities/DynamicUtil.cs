using System.Dynamic;

namespace Server.Web.Utilities
{
    internal class DynamicUtil
    {
        public static bool HasProperty(dynamic dynamicObject, string property)
        {
            if(dynamicObject == null)
                return false;

            if(dynamicObject is ExpandoObject)
                return ((IDictionary<string, object>)dynamicObject).ContainsKey(property);

            return dynamicObject.GetType().GetProperty(property) != null;
        }
    }
}
