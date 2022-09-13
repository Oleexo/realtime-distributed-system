using System.Collections.Concurrent;
using Prometheus;

namespace Oleexo.RealtimeDistributedSystem.Common.Metrics;

internal sealed class PrometheusMetrics : IMetrics {
    private readonly ConcurrentDictionary<string, Counter>   _counters;
    private readonly ConcurrentDictionary<string, Gauge>     _gauges;
    private readonly ConcurrentDictionary<string, Histogram> _histograms;

    public PrometheusMetrics() {
        _counters   = new ConcurrentDictionary<string, Counter>();
        _gauges     = new ConcurrentDictionary<string, Gauge>();
        _histograms = new ConcurrentDictionary<string, Histogram>();
    }

    public void CreateCounter(string key,
                              string description) {
        _counters[key] = Prometheus.Metrics.CreateCounter(key, description);
    }

    public void CreateGauge(string key,
                            string description) {
        _gauges[key] = Prometheus.Metrics.CreateGauge(key, description);
    }

    public void CreateHistogram(string key,
                                string description) {
        _histograms[key] = Prometheus.Metrics.CreateHistogram(key, description);
    }

    public IDisposable NewTimer(string key) {
        if (_histograms.TryGetValue(key, out var histogram)) {
            return histogram.NewTimer();
        }

        throw new InvalidOperationException("Missing histogram. Create first with CreateHistogram()");
    }
    public void IncrCounter(string key,
                            int    value = 1) {
        if (_counters.TryGetValue(key, out var counter)) {
            counter.Inc(value);
        }
        else {
            throw new InvalidOperationException("Missing counter. Create first with CreateCounter()");
        }
    }

    public void IncrGauge(string key,
                          int    value = 1) {
        if (_gauges.TryGetValue(key, out var gauge)) {
            gauge.Inc(value);
        }
        else {
            throw new InvalidOperationException("Missing counter. Create first with CreateGauge()");
        }
    }

    public void DecrGauge(string key,
                          int    value = -1) {
        if (_gauges.TryGetValue(key, out var gauge)) {
            gauge.Dec(value);
        }
        else {
            throw new InvalidOperationException("Missing counter. Create first with CreateGauge()");
        }
    }
}