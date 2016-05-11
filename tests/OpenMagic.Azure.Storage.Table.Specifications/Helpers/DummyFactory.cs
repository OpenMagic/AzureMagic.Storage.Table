using System;
using System.Net;
using OpenMagic.Extensions;

namespace OpenMagic.Azure.Storage.Table.Specifications.Helpers
{
    public class DummyFactory : Dummy
    {
        public DummyFactory()
        {
            ValueFactories.Add(typeof(HttpStatusCode), () => Enum.Parse(typeof(HttpStatusCode), typeof(HttpStatusCode).GetEnumNames().RandomItem()));
            ValueFactories.Add(typeof(DateTimeOffset?), () => DummyNullableDateTimeOffset());
        }

        private static DateTimeOffset? DummyNullableDateTimeOffset()
        {
            return RandomBoolean.Next() ? (DateTimeOffset?)null : new DateTimeOffset(RandomDateTime.Next());
        }
    }
}