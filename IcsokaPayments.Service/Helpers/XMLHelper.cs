using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace IcsokaPayments.Service.Helpers
{
    public class XmlHelper
    {
        
        private static readonly Regex s_xmlEncodePattern = new Regex(@"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", RegexOptions.Compiled);
        private static readonly XmlWriterSettings WriterSettings = new XmlWriterSettings { OmitXmlDeclaration = false, Indent = false , Encoding = Encoding.UTF8, NewLineOnAttributes = false, NewLineHandling = NewLineHandling.None};
        private static readonly XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", ""),XmlQualifiedName.Empty });


		#region Methods

		/// <summary>
		/// XML Encode
		/// </summary>
		/// <param name="str">String</param>
		/// <returns>Encoded string</returns>
		public static string XmlEncode(string str)
		{
			if (str == null)
				return null;

			str = s_xmlEncodePattern.Replace(str, "");
			return XmlEncodeAsIs(str);
		}

		/// <summary>
		/// XML Encode as is
		/// </summary>
		/// <param name="str">String</param>
		/// <returns>Encoded string</returns>
		public static string XmlEncodeAsIs(string str)
		{
			if (str == null)
				return null;

			using (var sw = new StringWriter())
			{
				using (var xwr = new XmlTextWriter(sw))
				{
					xwr.WriteString(str);
					return sw.ToString();
				}
			}
		}

		/// <summary>
		/// Encodes an attribute
		/// </summary>
		/// <param name="str">Attribute</param>
		/// <returns>Encoded attribute</returns>
		public static string XmlEncodeAttribute(string str)
		{
			if (str == null)
				return null;
			str = s_xmlEncodePattern.Replace(str, "");
			return XmlEncodeAttributeAsIs(str);
		}

		/// <summary>
		/// Encodes an attribute as is
		/// </summary>
		/// <param name="str">Attribute</param>
		/// <returns>Encoded attribute</returns>
		public static string XmlEncodeAttributeAsIs(string str)
		{
			return XmlEncodeAsIs(str).Replace("\"", "&quot;");
		}

		/// <summary>
		/// Decodes an attribute
		/// </summary>
		/// <param name="str">Attribute</param>
		/// <returns>Decoded attribute</returns>
		public static string XmlDecode(string str)
		{
			var sb = new StringBuilder(str);
			return sb.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").ToString();
		}

		/// <summary>
		/// Serializes a datetime
		/// </summary>
		/// <param name="dateTime">Datetime</param>
		/// <returns>Serialized datetime</returns>
		public static string SerializeDateTime(DateTime dateTime)
		{
			var xmlS = new XmlSerializer(typeof(DateTime));
			var sb = new StringBuilder();
			using (var sw = new StringWriter(sb))
			{
				xmlS.Serialize(sw, dateTime);
				return sb.ToString();
			}
		}

		/// <summary>
		/// Deserializes a datetime
		/// </summary>
		/// <param name="dateTime">Datetime</param>
		/// <returns>Deserialized datetime</returns>
		public static DateTime DeserializeDateTime(string dateTime)
		{
			var xmlS = new XmlSerializer(typeof(DateTime));
			using (var sr = new StringReader(dateTime))
			{
				object test = xmlS.Deserialize(sr);
				return (DateTime)test;
			}
		}

		/// <summary>
		/// Serializes an object instance to a XML formatted string
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="instance">Object instance</param>
		/// <returns>XML string</returns>
		public static string Serialize<T>(T instance)
		{
			return Serialize(instance, typeof(T));
		}

        /// <summary>
        /// Serializes an object instance to a XML formatted string
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="instance">Object instance</param>
        /// <returns>XML string</returns>
        public static string SerializeUtf8<T>(T instance)
        {
            return SerializeUtf8(instance, typeof(T));
        }


        /// <summary>
        /// Serializes an object instance to a XML formatted string
        /// </summary>
        /// <param name="instance">Object instance</param>
        /// <param name="type">Object type</param>
        /// <returns>XML string</returns>
        public static string SerializeUtf8(object instance, Type type)
        {
            if (instance == null)
                return null;

            if (type == null)
            {
                return null;
            }
            XmlSerializer xsSubmit = new XmlSerializer(type);
            StringWriter sw = new Utf8StringWriter(new StringBuilder());
            using (XmlWriter writer = XmlWriter.Create(sw, WriterSettings))
            {
                // removes namespace
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);

                xsSubmit.Serialize(writer, instance, xmlns);
                return sw.ToString(); // Your XML
            }

        }
		/// <summary>
		/// Serializes an object instance to a XML formatted string
		/// </summary>
		/// <param name="instance">Object instance</param>
		/// <param name="type">Object type</param>
		/// <returns>XML string</returns>
		public static string Serialize(object instance, Type type)
		{
			if (instance == null)
				return null;

		    if (type == null)
		    {
		        return null;
		    }


            using (var writer = new StringWriter())
            {
                var xmlSerializer = new XmlSerializer(type);

                xmlSerializer.Serialize(writer, instance);
                return writer.ToString();
            }
		   
		}

		/// <summary>
		/// Deserializes a XML formatted string to an object instance
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="xml">XML string</param>
		/// <returns>Object instance</returns>
		public static T Deserialize<T>(string xml)
		{
			return (T)Deserialize(xml, typeof(T));
		}

		/// <summary>
		/// Deserializes a XML formatted string to an object instance
		/// </summary>
		/// <param name="xml">XML string</param>
		/// <param name="type">Object type</param>
		/// <returns>Object instance</returns>
		public static object Deserialize(string xml, Type type)
		{
			

		    try
		    {
		        if (CommonHelper.HasValue(xml))
		        {
		            using (var reader = new StringReader(xml))
		            {
		                var serializer = new XmlSerializer(type);
		                return serializer.Deserialize(reader);
		            }
		        }
		    }
		    catch
		    {
		        throw;
		    }

		    return null;
		}

		#endregion
	}

    public class Utf8StringWriter : StringWriter
{
    public Utf8StringWriter(StringBuilder sb) : base (sb)
    {
    }
    public override Encoding Encoding { get { return Encoding.UTF8; } }
}
    }
