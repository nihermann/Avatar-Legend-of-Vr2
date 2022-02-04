using System.IO;
using System.Linq;
using UnityEngine;

public static class Snapshot
{
    public static void TakeCSVSnapshot(string filePath, params ISnapshotable[] snaps)
    {
        using var writer = new StreamWriter(Path.Join(Application.streamingAssetsPath, filePath));
        
        var headers = snaps.SelectMany(snap => snap.Header());
        var headerAsCsv = string.Join(",", headers);
        writer.WriteLine(headerAsCsv);
        var records = snaps.SelectMany(snap => snap.Record());
        var recordsAsCsv = string.Join(",", records);
        writer.WriteLine(recordsAsCsv);
    }
}
