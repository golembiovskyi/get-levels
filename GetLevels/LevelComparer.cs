using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace BIT
{
    internal class LevelComparer : IEqualityComparer<Level>
    {
        public bool Equals(Level x, Level y)
        {
            return x.Id.IntegerValue.Equals(y.Id.IntegerValue);
        }

        public int GetHashCode(Level obj)
        {
            return obj.Id.IntegerValue;
        }
    }
}
