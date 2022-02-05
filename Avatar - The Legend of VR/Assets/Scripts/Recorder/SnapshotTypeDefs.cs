using System;
using System.Collections.Generic;
using System.Linq;

public class SnapshotTypeDefs
{
    public static IEnumerable<string> CreateSnapshot(params object[] objects)
    {
        return objects.SelectMany(RecorderDefinitions.GenerateOutput);
    }

    public static IEnumerable<string> CreateHeaders(params Header[] objects)
    {
        return objects.SelectMany(obj => RecorderDefinitions.GenerateHeader(obj.Name, obj.Type.Name));
    }
    

    public struct Header
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public Header(string name, Type t)
        {
            Name = name;
            Type = t;
        }
    } 
    
}
