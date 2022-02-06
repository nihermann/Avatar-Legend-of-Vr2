using System.Collections.Generic;

public interface ISnapshotable
{
    public IEnumerable<IEnumerable<string>> Record();
    public IEnumerable<string> Header();
}
