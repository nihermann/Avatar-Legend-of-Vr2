using System.IO;
using System.Linq;
using UnityEngine;

public static class Snapshot
{
    public static void TakeCSVSnapshot(string filePath, ISnapshotable snaps)
    {
        using var writer = new StreamWriter(Path.Join(Application.streamingAssetsPath, filePath));
        
        var headerAsCsv = string.Join(",", snaps.Header());
        writer.WriteLine(headerAsCsv);
        foreach (var recs in snaps.Record())
        {
            var recordsAsCsv = string.Join(",", recs);
            writer.WriteLine(recordsAsCsv);
        }
    }
}
