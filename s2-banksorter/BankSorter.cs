using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;
using System.Xml;
using System.Xml.Linq;

namespace Program
{
    using COMPARATOR = Func<XmlNode, string>;
    public class XmlUtils
    {
        public static void SortXml(XmlDocument document)
        {
            SortXml(document.DocumentElement);
        }

        public static void SortXml(XmlNode rootNode)
        {
            COMPARATOR comparator = node => node.Attributes?["name"]?.Value ?? node.Name;

            SortElements(rootNode, comparator); //sort sections
            foreach (XmlNode childNode in rootNode.ChildNodes)
            {
                SortElements(childNode, comparator); //sort by keys
            }
        }

        //returns true if elements were sorted
        public static bool SortElements(XmlNode node, COMPARATOR comparator)
        {
            XmlNode nodeCopy = node.Clone(); //make copy otherwise indicies of node will change during loop
            var indicies = Enumerable.Range(0, nodeCopy.ChildNodes.Count).OrderBy(i => comparator(nodeCopy.ChildNodes[i])).ToArray();
            for(int i = 0; i < indicies.Length; ++i)
            {
                if(i != indicies[i])
                {
                    var newNode = nodeCopy.ChildNodes[indicies[i]].Clone();
                    var oldNode = node.ChildNodes[i];
                    node.ReplaceChild(newNode, oldNode);
                }
            }

            return true;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine($"Error: expected list of filenames to sort");
            }
            foreach(string filename in args)
            {
                if(!File.Exists(filename))
                {
                    Console.WriteLine($"Error: {filename} does not exist");
                    continue;
                }
                if(Path.GetExtension(filename).ToLower() != ".SC2Bank".ToLower())
                {
                    Console.WriteLine($"Error: {filename} is not a valid bank");
                    continue;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                if(doc.DocumentElement == null)
                {
                    Console.WriteLine($"Error: {filename} does not have a valid root element");
                    continue;
                }

                XmlUtils.SortXml(doc.DocumentElement);
                doc.Save(filename);
            }
        }
    }
}