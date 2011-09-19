using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace MerchantTribe.Commerce.Content
{
	public class ThemeInfo
	{

        private string _UniqueId = System.Guid.NewGuid().ToString();
		private string _Author = string.Empty;
		private string _AuthorUrl = string.Empty;
		private string _Description = string.Empty;
		private string _Title = "Unknown";
		private string _Version = "Unknown";
		private string _VersionUrl = "0.0.0";

        public string UniqueId
        {
            get { return _UniqueId; }
            set { _UniqueId = value; }
        }
        // Scrub out curl braces
        public string UniqueIdAsString
        {
            get
            {
                string result = _UniqueId.ToString().ToLower();
                result = result.Replace("{", "");
                result = result.Replace("}","");                
                return result;
            }
        }
		public string Author {
			get { return _Author; }
			set { _Author = value; }
		}
		public string AuthorUrl {
			get { return _AuthorUrl; }
			set { _AuthorUrl = value; }
		}
		public string Description {
			get { return _Description; }
			set { _Description = value; }
		}
		public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		public string Version {
			get { return _Version; }
			set { _Version = value; }
		}
		public string VersionUrl {
			get { return _VersionUrl; }
			set { _VersionUrl = value; }
		}

		public ThemeInfo()
		{

		}

		public static ThemeInfo ReadFromXml(string fileName)
		{
			ThemeInfo result = new ThemeInfo();

			try {
				if (File.Exists(fileName)) {
					string xmlData = File.ReadAllText(fileName);
					result.LoadFromString(xmlData);
				}
			}
			catch {
				// surpress errors
			}

			return result;
		}

		public bool LoadFromString(string data)
		{
			System.IO.StringReader sw = new System.IO.StringReader(data);
			XmlReader xr = XmlReader.Create(sw);
			bool result = LoadFromXmlReader(xr);
			sw.Dispose();
			xr.Close();
			return result;
		}

        public string WriteToXmlString()
        {
            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xw = new XmlTextWriter(sw))
                {
                    if (xw != null)
                    {
                        xw.WriteStartElement("BVTheme");

                        xw.WriteElementString("UniqueId", UniqueIdAsString);
                        xw.WriteElementString("Author", _Author);
                        xw.WriteElementString("AuthorUrl", _AuthorUrl);
                        xw.WriteElementString("Title", _Title);
                        xw.WriteElementString("Description", _Description);
                        xw.WriteElementString("Version", _Version);
                        xw.WriteElementString("VersionUrl", _VersionUrl);

                        xw.WriteEndElement();
                    }
                }
                return sw.ToString();
            }
        }
		private bool LoadFromXmlReader(XmlReader xr)
		{
			bool results = false;

			try {
				XPathDocument xdoc = new XPathDocument(xr);
				XPathNavigator nav = xdoc.CreateNavigator();

				if (nav.SelectSingleNode("/BVTheme") != null) {

                    //UniqueId
                    if (nav.SelectSingleNode("/BVTheme/UniqueId") != null)
                    {
                        _UniqueId = nav.SelectSingleNode("/BVTheme/UniqueId").Value;
                    }

					//Author
					if (nav.SelectSingleNode("/BVTheme/Author") != null) {
						_Author = nav.SelectSingleNode("/BVTheme/Author").Value;
					}

					//AuthorUrl
					if (nav.SelectSingleNode("/BVTheme/AuthorUrl") != null) {
						_AuthorUrl = nav.SelectSingleNode("/BVTheme/AuthorUrl").Value;
					}

					//Title
					if (nav.SelectSingleNode("/BVTheme/Title") != null) {
						_Title = nav.SelectSingleNode("/BVTheme/Title").Value;
					}

					//Description
					if (nav.SelectSingleNode("/BVTheme/Description") != null) {
						_Description = nav.SelectSingleNode("/BVTheme/Description").Value;
					}

					//Version
					if (nav.SelectSingleNode("/BVTheme/Version") != null) {
						_Version = nav.SelectSingleNode("/BVTheme/Version").Value;
					}

					//VersionUrl
					if (nav.SelectSingleNode("/BVTheme/VersionUrl") != null) {
						_VersionUrl = nav.SelectSingleNode("/BVTheme/VersionUrl").Value;
					}

					results = true;
				}
			}

			catch  {
				results = false;
			}

			return results;
		}

	}
}
