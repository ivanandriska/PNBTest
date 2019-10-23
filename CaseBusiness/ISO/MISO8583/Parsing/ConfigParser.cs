using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace CaseBusiness.ISO.MISO8583.Parsing
{
    /// <summary>
    /// This class can read a configuration file in XML format and configure
    /// a MessageFactory with the information contained in it. The config file
    /// can contain ISO headers for each message type, as well as templates
    /// for different message types and finally parsing guides for each
    /// message type. The message type must be indicated in hex.
    /// </summary>
    public class ConfigParser
    {
        public enum Base
        {
            Binary = 2,
            Octal = 8,
            Decimal = 10,
            Hexadecimal = 16,
            B64 = 64
        }

        public static Dictionary<Int32, CaseBusiness.ISO.IsoMessage> ISOTypeTemplates = new Dictionary<Int32, CaseBusiness.ISO.IsoMessage>();
        //public static List<Int32> ISOParseOrder = new List<Int32>();
        public static Dictionary<Int32, String> ISOHeaders = new Dictionary<Int32, String>();
        public static Int32 ISOLengthMTI = 4;
        public static Int32 ISOLengthBitMap = 16;
        public static Boolean ISOAssignDate = false;

        /// <summary>
        /// Tells the MessageFactory whether it should assign the date
        /// to newly created messages or not. Default is false. The
        /// date is field 7.
        /// </summary>
        public Boolean AssignDate
        {
            set { ISOAssignDate = value; }
            get { return ISOAssignDate; }
        }

        //public void SetLengthMTI(Int32 value)
        //{
        //    if (value != null)
        //    {
        //        ISOLengthMTI = value;
        //    }
        //}

        /// <summary>
        /// Sets the ISO message header to use when creating messages of the given
        /// type.
        /// </summary>
        /// <param name="type">The ISO message type (0x200, 0x400, etc)</param>
        /// <param name="value">The ISO header to use, or null to not use a header for the message type.</param>
        public void SetIsoHeader(Int32 type, String value)
        {
            if (value == null)
            {
                ISOHeaders.Remove(type);
            }
            else
            {
                ISOHeaders[type] = value;
            }
        }
        /// <summary>
        /// Returns the ISO message header for the given message type.
        /// </summary>
        /// <param name="type">The ISO message type (0x200, 0x400, etc)</param>
        public String GetIsoHeader(Int32 type)
        {
            return ISOHeaders[type];
        }

        /// <summary>
        /// Stores the given message as a template to create new messages of the
        /// same type as it. It overwrites any previously stored templates for
        /// the same message type.
        /// </summary>
        /// <param name="templ">A message to be used as a template.</param>
        public void AddMessageTemplate(IsoMessage templ)
        {
            if (templ != null)
            {
                ISOTypeTemplates[templ.Type] = templ;
            }
        }
        /// <summary>
        /// Removes the template for the given message type.
        /// </summary>
        /// <param name="type">The ISO message type (0x200, 0x400, etc)</param>
        public void RemoveMessageTemplate(Int32 type)
        {
            ISOTypeTemplates.Remove(type);
        }

        /// <summary>
        /// Sets a dictionary with the necessary information to parse a message
        /// of the given type. The dictionary contains FieldParseInfo objects
        /// under the field index they correspond to.
        /// </summary>
        /// <param name="dict">The FieldParseInfo objects for parsing individual fields under the field index they correspond to.</param>
        //public void SetParseDictionary(Dictionary<Int32, FieldParseInfo> dict)
        //{
        //    //ISOParseMap = dict;
        //    List<Int32> index = new List<Int32>();

        //    /////Agregar todas las llaves del dict a index
        //    /////ordenar index por numeros
        //    for (Int32 i = 2; i < 129; i++)
        //    {
        //        if (dict.ContainsKey(i))
        //            index.Add(i);
        //    }

        //    ISOParseOrder = index;
        //}


        /// <summary>
        /// Creates a MessageFactory and configures it from the XML file read
        /// from the absolute path specified.
        /// </summary>
        /// <param name="filename">An absolute path to the XML configuration file.</param>
        /// <returns>A new configured MessageFactory.</returns>
      //  public void LoadParseMapOriginal(CaseBusiness.ISO.EncodingType encodingType,  String filename)
      //  {
      //      XmlDocument xml = new XmlDocument();

      //      xml.Load(filename);

      //      foreach (XmlNode node in xml.DocumentElement.ChildNodes)
      //      {
      //          if (node.NodeType != XmlNodeType.Element) { continue; }

      //          if (node.Name == "lengthMTI")
      //          {
      //              Int32 lengthMTI = Convert.ToInt32(node.Attributes["value"].Value);

      //              SetLengthMTI(lengthMTI);
      //          }

      //          if (node.Name == "header")
      //          {
      //              String type   = node.Attributes["type"].Value;
      //              String header = (node.ChildNodes[0] != null) ? node.ChildNodes[0].Value : String.Empty;

      //              SetIsoHeader(Convert.ToInt32(type, 16), header);
      //              continue;
      //          }
                
      //          if (node.Name == "template")
      //          {
      //              IsoMessage templ = new IsoMessage();

      //              templ.Type = Convert.ToInt32(node.Attributes["type"].Value, 16);
                    
      //              foreach (XmlNode field in node.ChildNodes)
      //              {
                        //String val = ""; 
                        //String ftype = "";
                        //Int32 num = 0;
                        //String length = "";
                        //Dictionary<Int32, IsoValue> sub = null;	
                        //String requirements = "";


                        //ftype = field.Attributes["type"].Value;
                        //num = Convert.ToInt32(field.Attributes["num"].Value);
                        //length = (field.Attributes["length"] == null) ? "0" : field.Attributes["length"].Value;

                        //if (field.Attributes["requirements"] != null)
                        //	requirements = field.Attributes["requirements"].Value;


                        //if (field.NodeType != XmlNodeType.Element || field.Name != "field") { continue; }

                        //if (field.HasChildNodes)
                        //{
                        //	sub = new Dictionary<Int32, IsoValue>();

                        //	foreach (XmlNode subData in field.ChildNodes)
                        //	{
      //                          Dictionary<Int32, IsoValue> subSub = new Dictionary<Int32, IsoValue>();

                        //		String valSub = "";
                        //		String ftypeSub = "";
                        //		String numSub = "";
                        //		String lengthSub = "";
                        //		Int32 lenIDSub = 0;
                        //		Int32 lenLenSub = 0;
                        //		String requirementsSub = "";

                        //		if (subData.NodeType != XmlNodeType.Comment)
                        //		{
                        //			TypeElement typeElement;
                        //			if (subData.Name == "subfield")
                        //			{
                        //				typeElement = TypeElement.SubField;

                        //				valSub = subData.Attributes["value"].Value;
                        //				ftypeSub = subData.Attributes["type"].Value;
                        //				numSub = subData.Attributes["num"].Value;
                        //				lengthSub = (subData.Attributes["length"] == null) ? "0" : subData.Attributes["length"].Value;

                        //				if (subData.Attributes["requirements"] != null)
                        //					requirementsSub = subData.Attributes["requirements"].Value;
                        //			}
                        //			else
                        //			{
                        //				typeElement = TypeElement.SubElement;
                        //				ftypeSub = subData.Attributes["type"].Value;
                        //				numSub = subData.Attributes["num"].Value;
                        //				lengthSub = (subData.Attributes["length"] == null) ? "0" : subData.Attributes["length"].Value;

                        //				if (subData.Attributes["requirements"] != null)
                        //					requirementsSub = subData.Attributes["requirements"].Value;

                        //				if (subData.Attributes["subElementIDLength"] != null)
                        //					lenIDSub = Convert.ToInt32(subData.Attributes["subElementIDLength"].Value);

                        //				if (subData.Attributes["lengthOfLengthSubElement"] != null)
                        //					lenLenSub = Convert.ToInt32(subData.Attributes["lengthOfLengthSubElement"].Value);

      //                                  foreach (XmlNode subSubData in subData.ChildNodes)
      //                                  {
      //                                      if (subSubData.NodeType != XmlNodeType.Comment)
      //                                      {
      //                                          String valSubSub = "";
      //                                          Int32 lenSubSub = 0;
      //                                          Int32 lenIDSubSub = 0;
      //                                          Int32 lenLenSubSub = 0;
      //                                          Int32 numSubSub = 0;
      //                                          String requirementsSubSub = "";
                                                
      //                                          numSubSub = Convert.ToInt32(subSubData.Attributes["num"].Value);

      //                                          String ftypeSubSub = subSubData.Attributes["type"].Value;
      //                                          TypeElement typeElementSub;

      //                                          if (subSubData.Name == "subfield")
      //                                          {
      //                                              typeElementSub = TypeElement.SubField;

      //                                              valSubSub = subSubData.Attributes["value"].Value;
      //                                              ftypeSubSub = subSubData.Attributes["type"].Value;
      //                                              numSubSub = Convert.ToInt32(subSubData.Attributes["num"].Value);
      //                                              lenSubSub = (subSubData.Attributes["length"] == null) ? 0 : Convert.ToInt32(subSubData.Attributes["length"].Value);
      //                                          }
      //                                          else
      //                                          {
      //                                              typeElementSub = TypeElement.SubElement;
      //                                              if (subSubData.Attributes["subElementIDLength"] != null)
      //                                                  lenIDSubSub = Convert.ToInt32(subSubData.Attributes["subElementIDLength"].Value);

      //                                              if (subSubData.Attributes["lengthOfLengthSubElement"] != null)
      //                                                  lenLenSubSub = Convert.ToInt32(subSubData.Attributes["lengthOfLengthSubElement"].Value);
      //                                          }

      //                                          if (subSubData.Attributes["requirements"] != null)
      //                                              requirementsSubSub = subSubData.Attributes["requirements"].Value;


      //                                          subSub.Add(Convert.ToInt32(numSubSub), new IsoValue((IsoType)Enum.Parse(typeof(IsoType), ftypeSubSub), valSubSub, Convert.ToInt16(lenSubSub), valSubSub, requirementsSubSub, typeElementSub, lenIDSubSub, lenLenSubSub));
      //                                      }
      //                                  }
                        //			}
                        //			sub.Add(Convert.ToInt32(numSub), new IsoValue((IsoType)Enum.Parse(typeof(IsoType), ftypeSub), valSub, Convert.ToInt16(lengthSub), valSub, requirementsSub, typeElement, subSub, lenIDSub, lenLenSub));
                        //		}
                        //	}
                        //}
                        //else
                        //{
                        //	val = field.Attributes["value"].Value;
                        //}
                        //templ.SetValue(num, val, (IsoType)Enum.Parse(typeof(IsoType), ftype), Convert.ToInt16(length), requirements, sub, TypeElement.Field);
      //              }

      //              AddMessageTemplate(templ);

      //              continue;
      //          }
                
      //          if (node.Name == "parse")
      //          {
      //              Dictionary<Int32, FieldParseInfo> guide = new Dictionary<Int32, FieldParseInfo>();

      //              Int32 type = Convert.ToInt16(node.Attributes["type"].Value, 16);

      //              foreach (XmlNode field in node.ChildNodes)
      //              {
                        //Dictionary<Int32, FieldParseInfo> sub = new Dictionary<Int32, FieldParseInfo>();

      //                  if ((field.NodeType != XmlNodeType.Element && field.NodeType != XmlNodeType.Comment) || field.Name == "field")
      //                  {
      //                      Int32 len = 0;
      //                      String num = field.Attributes["num"].Value;
      //                      String ftype = field.Attributes["type"].Value;
      //                      Boolean req = false;
                        //	Base b = Base.Decimal;
                        //	CaseBusiness.ISO.EncodingType enc = encodingType;

                        //	if (field.Attributes["length"] != null)
                        //		len = Convert.ToInt16(field.Attributes["length"].Value);

                        //	if (field.Attributes["required"] != null)
                        //		req = Convert.ToBoolean(field.Attributes["required"].Value);

                        //	if (field.Attributes["base"] != null)
                        //		b = (Base)Convert.ToInt32(field.Attributes["base"].Value);

      //                      //if (field.Attributes["encoding"] != null)
      //                      //    enc = (field.Attributes["encoding"].Value == "EBCDIC" ? CaseBusiness.ISO.EncodingType.EBCDIC : CaseBusiness.ISO.EncodingType.ASCII );

      //                      foreach (XmlNode subData in field.ChildNodes)
      //                      {
                        //		Dictionary<Int32, FieldParseInfo> subSub = new Dictionary<Int32, FieldParseInfo>();

                        //		if (subData.NodeType != XmlNodeType.Comment)
                        //		{
                        //			Int32 lenSub = 0;
                        //			Int32 lenIDSub = 0;
                        //			Int32 lenLenSub = 0;
                        //			Int32 numSub = 0;
                        //			if (b == Base.Hexadecimal)
                        //				numSub = Convert.ToInt32(subData.Attributes["num"].Value, 16);
                        //			else
                        //				numSub = Convert.ToInt32(subData.Attributes["num"].Value);

                        //			String ftypeSub = subData.Attributes["type"].Value;
                        //			Boolean reqSub = false;
                        //			TypeElement typeElement;
                        //			FieldParseInfo fpiSub;

                        //			if (subData.Name == "subfield")
                        //				typeElement = TypeElement.SubField;
                        //			else
                        //			{
                        //				typeElement = TypeElement.SubElement;
                        //				if (subData.Attributes["subElementIDLength"] != null)
                        //					lenIDSub = Convert.ToInt32(subData.Attributes["subElementIDLength"].Value);

                        //				if (subData.Attributes["lengthOfLengthSubElement"] != null)
                        //					lenLenSub = Convert.ToInt32(subData.Attributes["lengthOfLengthSubElement"].Value);
                        //			}

                        //			if (subData.Attributes["length"] != null)
                        //				lenSub = Convert.ToInt16(subData.Attributes["length"].Value);

                        //			if (subData.Attributes["required"] != null)
                        //				reqSub = Convert.ToBoolean(subData.Attributes["required"].Value);

                        //			foreach (XmlNode subSubData in subData.ChildNodes)
                        //			{
                        //				if (subSubData.NodeType != XmlNodeType.Comment)
                        //				{
                        //					Int32 lenSubSub = 0;
                        //					Int32 lenIDSubSub = 0;
                        //					Int32 lenLenSubSub = 0;
                        //					Int32 numSubSub = 0;
                        //					if (b == Base.Hexadecimal)
                        //						numSubSub = Convert.ToInt32(subSubData.Attributes["num"].Value, 16);
                        //					else
                        //						numSubSub = Convert.ToInt32(subSubData.Attributes["num"].Value);

                        //					String ftypeSubSub = subSubData.Attributes["type"].Value;
                        //					Boolean reqSubSub = false;
                        //					TypeElement typeElementSub;
                        //					FieldParseInfo fpiSubSub;

                        //					if (subSubData.Name == "subfield")
                        //						typeElementSub = TypeElement.SubField;
                        //					else
                        //					{
                        //						typeElementSub = TypeElement.SubElement;
                        //						if (subSubData.Attributes["subElementIDLength"] != null)
                        //							lenIDSubSub = Convert.ToInt32(subSubData.Attributes["subElementIDLength"].Value);

                        //						if (subSubData.Attributes["lengthOfLengthSubElement"] != null)
                        //							lenLenSubSub = Convert.ToInt32(subSubData.Attributes["lengthOfLengthSubElement"].Value);
                        //					}

                        //					if (subSubData.Attributes["length"] != null)
                        //						lenSubSub = Convert.ToInt16(subSubData.Attributes["length"].Value);

                        //					if (subSubData.Attributes["required"] != null)
                        //						reqSubSub = Convert.ToBoolean(subSubData.Attributes["required"].Value);

                        //					fpiSubSub = new FieldParseInfo((IsoType)Enum.Parse(typeof(IsoType), ftypeSubSub), lenSubSub, reqSubSub, typeElementSub, null, lenIDSubSub, lenLenSubSub);

                        //					subSub.Add(numSubSub, fpiSubSub);
                        //				}
                        //			}

                        //			fpiSub = new FieldParseInfo((IsoType)Enum.Parse(typeof(IsoType), ftypeSub), lenSub, reqSub, typeElement, subSub, lenIDSub, lenLenSub);

                        //			sub.Add(numSub, fpiSub);
                        //		}
      //                      }

      //                      //TODO ir creando guia con esto
                        //	FieldParseInfo fpi = new FieldParseInfo((IsoType)Enum.Parse(typeof(IsoType), ftype), len, req, TypeElement.Field, sub, b, enc);
      //                      guide[Convert.ToInt16(num)] = fpi;
      //                  }
      //              }

      //              SetParseDictionary(type, guide);

      //              continue;
      //          }
                
      //          if (node.Name == "assign-date")
      //          {
      //              String val = node.Attributes["value"].Value;
      //              AssignDate = (val == "true");

      //              continue;
      //          }
      //      }

      //      //return m;----------------
      //  }


        //public void LoadParseMap(CaseBusiness.ISO.EncodingType encodingType, String filename)
        //{
        //    XmlDocument xml = new XmlDocument();

        //    xml.Load(filename);

        //    foreach (XmlNode node in xml.DocumentElement.ChildNodes)
        //    {
        //        if (node.NodeType != XmlNodeType.Element) { continue; }

        //        if (node.Name == "lengthMTI")
        //        {
        //            Int32 lengthMTI = Convert.ToInt32(node.Attributes["value"].Value);

        //            SetLengthMTI(lengthMTI);
        //        }

        //        if (node.Name == "header")
        //        {
        //            String type = node.Attributes["type"].Value;
        //            String header = (node.ChildNodes[0] != null) ? node.ChildNodes[0].Value : String.Empty;

        //            SetIsoHeader(Convert.ToInt32(type, 16), header);
        //            continue;
        //        }

        //        if (node.Name == "template")
        //        {
        //            IsoMessage templ = new IsoMessage();

        //            templ.Type = Convert.ToInt32(node.Attributes["type"].Value, 16);

        //            foreach (XmlNode field in node.ChildNodes)
        //            {
        //                String val = "";
        //                String ftype = "";
        //                Int32 num = 0;
        //                String length = "";
        //                Dictionary<Int32, IsoValue> sub = null;
        //                String requirements = "";
        //                Base b = Base.Decimal;


        //                ftype = field.Attributes["type"].Value;
        //                num = Convert.ToInt32(field.Attributes["num"].Value);
        //                length = (field.Attributes["length"] == null) ? "0" : field.Attributes["length"].Value;

        //                if (field.Attributes["requirements"] != null)
        //                    requirements = field.Attributes["requirements"].Value;

        //                if (field.Attributes["base"] != null)
        //                    b = (Base)Convert.ToInt32(field.Attributes["base"].Value);


        //                if (field.NodeType != XmlNodeType.Element || field.Name != "field") { continue; }

        //                if (field.HasChildNodes)
        //                {
        //                    sub = new Dictionary<Int32, IsoValue>();

        //                    foreach (XmlNode subData in field.ChildNodes)
        //                    {
        //                        Dictionary<Int32, IsoValue> subSub = new Dictionary<Int32, IsoValue>();

        //                        String valSub = "";
        //                        String ftypeSub = "";
        //                        String numSub = "";
        //                        String lengthSub = "";
        //                        Int32 lenIDSub = 0;
        //                        Int32 lenLenSub = 0;
        //                        String requirementsSub = "";

        //                        if (subData.NodeType != XmlNodeType.Comment)
        //                        {
        //                            TypeElement typeElement;
        //                            if (subData.Name == "subfield")
        //                            {
        //                                typeElement = TypeElement.SubField;

        //                                valSub = subData.Attributes["value"].Value;
        //                                ftypeSub = subData.Attributes["type"].Value;
        //                                numSub = subData.Attributes["num"].Value;
        //                                lengthSub = (subData.Attributes["length"] == null) ? "0" : subData.Attributes["length"].Value;

        //                                if (subData.Attributes["requirements"] != null)
        //                                    requirementsSub = subData.Attributes["requirements"].Value;
        //                            }
        //                            else
        //                            {
        //                                typeElement = TypeElement.SubElement;
        //                                ftypeSub = subData.Attributes["type"].Value;
        //                                numSub = subData.Attributes["num"].Value;
        //                                lengthSub = (subData.Attributes["length"] == null) ? "0" : subData.Attributes["length"].Value;

        //                                if (subData.Attributes["requirements"] != null)
        //                                    requirementsSub = subData.Attributes["requirements"].Value;

        //                                if (subData.Attributes["subElementIDLength"] != null)
        //                                    lenIDSub = Convert.ToInt32(subData.Attributes["subElementIDLength"].Value);

        //                                if (subData.Attributes["lengthOfLengthSubElement"] != null)
        //                                    lenLenSub = Convert.ToInt32(subData.Attributes["lengthOfLengthSubElement"].Value);

        //                                foreach (XmlNode subSubData in subData.ChildNodes)
        //                                {
        //                                    if (subSubData.NodeType != XmlNodeType.Comment)
        //                                    {
        //                                        String valSubSub = "";
        //                                        Int32 lenSubSub = 0;
        //                                        Int32 lenIDSubSub = 0;
        //                                        Int32 lenLenSubSub = 0;
        //                                        Int32 numSubSub = 0;
        //                                        String requirementsSubSub = "";

        //                                        numSubSub = Convert.ToInt32(subSubData.Attributes["num"].Value);

        //                                        String ftypeSubSub = subSubData.Attributes["type"].Value;
        //                                        TypeElement typeElementSub;

        //                                        if (subSubData.Name == "subfield")
        //                                        {
        //                                            typeElementSub = TypeElement.SubField;

        //                                            valSubSub = subSubData.Attributes["value"].Value;
        //                                            ftypeSubSub = subSubData.Attributes["type"].Value;
        //                                            numSubSub = Convert.ToInt32(subSubData.Attributes["num"].Value);
        //                                            lenSubSub = (subSubData.Attributes["length"] == null) ? 0 : Convert.ToInt32(subSubData.Attributes["length"].Value);
        //                                        }
        //                                        else
        //                                        {
        //                                            typeElementSub = TypeElement.SubElement;
        //                                            if (subSubData.Attributes["subElementIDLength"] != null)
        //                                                lenIDSubSub = Convert.ToInt32(subSubData.Attributes["subElementIDLength"].Value);

        //                                            if (subSubData.Attributes["lengthOfLengthSubElement"] != null)
        //                                                lenLenSubSub = Convert.ToInt32(subSubData.Attributes["lengthOfLengthSubElement"].Value);
        //                                        }

        //                                        if (subSubData.Attributes["requirements"] != null)
        //                                            requirementsSubSub = subSubData.Attributes["requirements"].Value;


        //                                        subSub.Add(Convert.ToInt32(numSubSub), new IsoValue((IsoType)Enum.Parse(typeof(IsoType), ftypeSubSub), b, valSubSub, Convert.ToInt16(lenSubSub), valSubSub, requirementsSubSub, typeElementSub, lenIDSubSub, lenLenSubSub));
        //                                    }
        //                                }
        //                            }
        //                            sub.Add(Convert.ToInt32(numSub), new IsoValue((IsoType)Enum.Parse(typeof(IsoType), ftypeSub), b, valSub, Convert.ToInt16(lengthSub), valSub, requirementsSub, typeElement, subSub, lenIDSub, lenLenSub));
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    val = field.Attributes["value"].Value;
        //                }
        //                templ.SetValue(b, num, val, (IsoType)Enum.Parse(typeof(IsoType), ftype), Convert.ToInt16(length), requirements, sub, TypeElement.Field);
        //            }

        //            AddMessageTemplate(templ);

        //            continue;
        //        }

        //        if (node.Name == "parse")
        //        {
        //            Dictionary<Int32, FieldParseInfo> guide = new Dictionary<Int32, FieldParseInfo>();

        //            //Int32 type = Convert.ToInt16(node.Attributes["type"].Value, 16);

        //            foreach (XmlNode field in node.ChildNodes)
        //            {
        //                Dictionary<Int32, FieldParseInfo> sub = null;

        //                if ((field.NodeType != XmlNodeType.Element && field.NodeType != XmlNodeType.Comment) || field.Name == "field")
        //                {
        //                    Int32 len = 0;
        //                    String num = field.Attributes["num"].Value;
        //                    String ftype = field.Attributes["type"].Value;
        //                    Boolean req = false;
        //                    Base b = Base.Decimal;
        //                    CaseBusiness.ISO.EncodingType enc = encodingType;

        //                    if (field.Attributes["length"] != null)
        //                        len = Convert.ToInt16(field.Attributes["length"].Value);

        //                    if (field.Attributes["required"] != null)
        //                        req = Convert.ToBoolean(field.Attributes["required"].Value);

        //                    if (field.Attributes["base"] != null)
        //                        b = (Base)Convert.ToInt32(field.Attributes["base"].Value);

        //                    if (field.HasChildNodes)
        //                        sub = LoadDataElement_Field(b, field.ChildNodes);

        //                    //TODO ir creando guia con esto
        //                    FieldParseInfo fpi = new FieldParseInfo((IsoType)Enum.Parse(typeof(IsoType), ftype), len, req, TypeElement.Field, sub, b, enc);
        //                    guide[Convert.ToInt16(num)] = fpi;
        //                }
        //            }

        //            SetParseDictionary(guide);

        //            continue;
        //        }

        //        if (node.Name == "assign-date")
        //        {
        //            String val = node.Attributes["value"].Value;
        //            AssignDate = (val == "true");

        //            continue;
        //        }
        //    }

        //    //return m;----------------
        //}

        //private Dictionary<Int32, FieldParseInfo> LoadDataElement_Field(ConfigParser.Base b, XmlNodeList xmlNodeList)
        //{
        //    Dictionary<Int32, FieldParseInfo> sub = new Dictionary<Int32, FieldParseInfo>();

        //    foreach (XmlNode subData in xmlNodeList)
        //    {
        //        if (subData.NodeType != XmlNodeType.Comment)
        //        {
        //            Int32 lenSub = 0;
        //            Int32 lenIDSub = 0;
        //            Int32 lenLenSub = 0;
        //            Int32 numSub = 0;
        //            if (b == Base.Hexadecimal)
        //                numSub = Convert.ToInt32(subData.Attributes["num"].Value, 16);
        //            else
        //                numSub = Convert.ToInt32(subData.Attributes["num"].Value);

        //            String ftypeSub = subData.Attributes["type"].Value;
        //            Boolean reqSub = false;
        //            TypeElement typeElement;
        //            FieldParseInfo fpiSub;

        //            if (subData.Name == "subfield")
        //                typeElement = TypeElement.SubField;
        //            else
        //            {
        //                typeElement = TypeElement.SubElement;
        //                if (subData.Attributes["subElementIDLength"] != null)
        //                    lenIDSub = Convert.ToInt32(subData.Attributes["subElementIDLength"].Value);

        //                if (subData.Attributes["lengthOfLengthSubElement"] != null)
        //                    lenLenSub = Convert.ToInt32(subData.Attributes["lengthOfLengthSubElement"].Value);
        //            }

        //            if (subData.Attributes["length"] != null)
        //                lenSub = Convert.ToInt16(subData.Attributes["length"].Value);

        //            if (subData.Attributes["required"] != null)
        //                reqSub = Convert.ToBoolean(subData.Attributes["required"].Value);

        //            if (subData.HasChildNodes)
        //                fpiSub = new FieldParseInfo((IsoType)Enum.Parse(typeof(IsoType), ftypeSub), lenSub, reqSub, typeElement, LoadDataElement_Field(b, subData.ChildNodes), lenIDSub, lenLenSub);
        //            else
        //                fpiSub = new FieldParseInfo((IsoType)Enum.Parse(typeof(IsoType), ftypeSub), lenSub, reqSub, typeElement, null, lenIDSub, lenLenSub);

        //            sub.Add(numSub, fpiSub);
        //        }
        //    }

        //    return sub;
        //}
    }
}