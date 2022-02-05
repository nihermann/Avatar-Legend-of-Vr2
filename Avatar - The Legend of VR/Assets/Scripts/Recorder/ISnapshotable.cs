using System.Collections.Generic;

public interface ISnapshotable
{
    public IEnumerable<string> Record();
    public IEnumerable<string> Header();
}
