﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Infrastructure.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReviewStateEntity
    {
        Active,
        Done,
        Postpone
    }
}
