using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SDLSample
{
    public static class ExtensionMethods
    {
        public static FieldInfo[] GetConstants(this Type type)
        {
            var constants = new List<FieldInfo>();

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            // Go through the list and only pick out the constants
            foreach (FieldInfo fi in fieldInfos)
            {
                // IsLiteral determines if its value is written at 
                //   compile time and not changeable
                // IsInitOnly determines if the field can be set 
                //   in the body of the constructor
                // for C# a field which is readonly keyword would have both true 
                //   but a const field would have only IsLiteral equal to true
                if (fi.IsLiteral && !fi.IsInitOnly)
                    constants.Add(fi);
            }


            // Return an array of FieldInfos
            return (FieldInfo[])constants.ToArray();
        }
    }
}
