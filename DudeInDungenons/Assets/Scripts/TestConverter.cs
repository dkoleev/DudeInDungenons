﻿using System;
using Newtonsoft.Json;

public class TestConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType) {
        throw new NotImplementedException();
    }
}
