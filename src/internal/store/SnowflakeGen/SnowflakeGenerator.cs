using IdGen;

namespace Oleexo.RealtimeDistributedSystem.Store.SnowflakeGen;

internal class SnowflakeGenerator : ISnowflakeGen {
    private readonly IdGenerator _generator;

    public SnowflakeGenerator() {
        _generator = new IdGenerator(0);
    }

    public long GetNewSnowflakeId() {
        return _generator.CreateId();
    }
}
