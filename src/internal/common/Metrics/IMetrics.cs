namespace Oleexo.RealtimeDistributedSystem.Common.Metrics;

public interface IMetrics {
    void CreateCounter(string key,
                       string description);

    void CreateGauge(string key,
                     string description);

    void CreateHistogram(string key,
                         string description);

    IDisposable NewTimer(string key);

    void IncrCounter(string key,
                     int    value = 1);

    void IncrGauge(string key,
                   int    value = 1);

    void DecrGauge(string key,
                   int    value = -1);
}