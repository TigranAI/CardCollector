using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CardCollector.Others
{
    public class JsonValueConverter<T> : ValueConverter<T, string>
    {
        private static Expression<Func<T, string>> ConvertToString = (value) => Utilities.ToJson(value);
        private static Expression<Func<string, T>> ConvertFromString = (value) => Utilities.FromJson<T>(value);
        
        public JsonValueConverter() : base(ConvertToString, ConvertFromString)
        {
        }
    }
}