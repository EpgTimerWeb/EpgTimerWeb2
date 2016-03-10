/*
 *  EpgTimerWeb2
 *  Copyright (C) 2016 EpgTimerWeb <webmaster@epgtimerweb.net>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;

namespace EpgTimer
{
    // by DynamicJson
    public class JsonUtil : DynamicObject
    {
        private enum JsonType
        {
            @string, number, boolean, @object, array, @null, date
        }
        public static string Serialize(object obj, bool Indent = true)
        {
            return CreateJsonString(new XStreamingElement("root", CreateTypeAttr(GetJsonType(obj)), CreateJsonNode(obj)), Indent);
        }
        private static object CreateJsonNode(object obj)
        {
            var type = GetJsonType(obj);
            switch (type)
            {
                case JsonType.@string:
                case JsonType.number:
                    return obj;
                case JsonType.date:
                    return UnixTime.ToUnixTime((DateTime)obj) * 1000;
                case JsonType.boolean:
                    return obj.ToString().ToLower();
                case JsonType.@object:
                    return CreateXObject(obj);
                case JsonType.array:
                    return CreateXArray(obj as IEnumerable);
                case JsonType.@null:
                default:
                    return null;
            }
        }
        private static JsonType GetJsonType(object obj)
        {
            if (obj == null) return JsonType.@null;

            switch (Type.GetTypeCode(obj.GetType()))
            {
                case TypeCode.Boolean:
                    return JsonType.boolean;
                case TypeCode.String:
                case TypeCode.Char:
                    return JsonType.@string;
                case TypeCode.DateTime:
                    return JsonType.date;
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return JsonType.number;
                case TypeCode.Object:
                    return (obj is IEnumerable) ? JsonType.array : JsonType.@object;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                default:
                    return JsonType.@null;
            }
        }
        private static XAttribute CreateTypeAttr(JsonType Type)
        {
            if (Type == JsonType.date) return new XAttribute("type", "number");
            return new XAttribute("type", Type.ToString());
        }
        private static IEnumerable<XStreamingElement> CreateXArray<T>(T obj) where T : IEnumerable
        {
            return obj.Cast<object>()
                .Select(o => new XStreamingElement("item", CreateTypeAttr(GetJsonType(o)), CreateJsonNode(o)));
        }
        private static IEnumerable<XStreamingElement> CreateXObject(object obj)
        {
            return obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(pi => new { Name = pi.Name, Value = pi.GetValue(obj, null) })
                .Where(n => n.Value != null)
                .Where(p => p.Value is string ? (string)p.Value != "" : true)
                .Select(a => new XStreamingElement(a.Name, CreateTypeAttr(GetJsonType(a.Value)), CreateJsonNode(a.Value)));
        }
        private static string CreateJsonString(XStreamingElement element, bool Indent)
        {
            using (var ms = new MemoryStream())
            using (var writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, Indent, "\t"))
            {
                element.WriteTo(writer);
                writer.Flush();
                return Encoding.UTF8.GetString(ms.ToArray())
                    .Replace("\\u000a", "\\n").Replace("\\u000d", "\\r").Replace("\\/", "/");
            }
        }
        readonly XElement xml;
        readonly JsonType jsonType;
        private JsonUtil(XElement element, JsonType type)
        {
            Debug.Assert(type == JsonType.array || type == JsonType.@object);

            xml = element;
            jsonType = type;
        }
    }
}
