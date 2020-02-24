using System;
using System.Threading;

namespace Infrastructure
{
    public class IdentityGenerator
    {
        private int _i;
        private static readonly DateTime StartTime = DateTime.Parse("2020/1/1");

        public IdentityGenerator(IdentityGeneratorOptions options)
        {
            Options = options;
        }

        public IdentityGeneratorOptions Options { get; }

        public long Generate()
        {
            var ms = (DateTime.UtcNow.Ticks - StartTime.Ticks) / 10000;
            Interlocked.CompareExchange(ref _i, -1, 4095);
            var seq = Interlocked.Increment(ref _i);
            var value = (ms << 22) | ((long)Options.InstanceTag << 12) | (long)seq;

            return value;
        }
    }
}