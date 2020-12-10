using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CodeLibrary.PluginPack
{
    [Description("Indents and formats XML")]
    public class XmlFormatter : IEditorPlugin
    {
        public string Category => "Formatting";
        public string DisplayName => "Xml Formatter";
        public Guid Id => Guid.Parse("b138e50a-cf6f-4ac3-bf72-a5e1233a21d3");
        public Image Image => ImageResource.xml;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = FormatXml(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }

        private static string FormatXml(string xml)
        {
            string result = "";

            using (MemoryStream mStream = new MemoryStream())
            {
                using (XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode))
                {
                    XmlDocument document = new XmlDocument();
                    try
                    {
                        // Load the XmlDocument with the XML.
                        document.LoadXml(xml);

                        writer.Formatting = Formatting.Indented;

                        // Write the XML into a formatting XmlTextWriter
                        document.WriteContentTo(writer);
                        writer.Flush();
                        mStream.Flush();

                        // Have to rewind the MemoryStream in order to read
                        // its contents.
                        mStream.Position = 0;

                        // Read MemoryStream contents into a StreamReader.
                        StreamReader sReader = new StreamReader(mStream);

                        // Extract the text from the StreamReader.
                        string formattedXml = sReader.ReadToEnd();

                        result = formattedXml;
                    }
                    catch (XmlException)
                    {
                        // Handle the exception
                    }
                    return result;
                }
            }
        }
    }
}