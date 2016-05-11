using System;
using System.Collections.Generic;
using System.Net;

namespace OpenMagic.Azure.Storage.Table.Specifications.Helpers.Dummies
{
    internal class DummyEntityWithTypes
    {
        public string StringValue { get; set; }
        public int IntegerValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public bool BooleanValue { get; set; }
        public DummyEntity ClassValue { get; set; }
        public List<DummyEntity> ListValue { get; set; }
        public HttpStatusCode EnumValue { get; set; }
    }
}