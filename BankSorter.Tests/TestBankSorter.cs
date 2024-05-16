//namespace BankSorter.Tests;
//using Program;
//using System;
//using System.Reflection.Metadata;
//using System.Xml;
//using Xunit.Abstractions;

//public class TestBankSorter
//{
//    private readonly ITestOutputHelper testOutputHelper;

//    public TestBankSorter(ITestOutputHelper testOutputHelper)
//    public TestBankSorter(ITestOutputHelper testOutputHelper)
//    {
//        this.testOutputHelper = testOutputHelper;
//    }

//    private bool ValidateSorted(XmlNode node)
//    {
//        if(node == null)
//        {
//            return true;
//        }

//        Func<XmlNode, string> comparator = node => node.Attributes?["name"]?.Value ?? node.Name;
//        var indicies = Enumerable.Range(0, node.ChildNodes.Count).OrderBy(i => comparator(node.ChildNodes[i])).ToArray();

//        if(!Enumerable.SequenceEqual(indicies, Enumerable.Range(0, node.ChildNodes.Count)))
//        {
//            testOutputHelper.WriteLine($"Failed indicies={string.Join(", ", indicies)} node={node.OuterXml}");
//            return false;
//        }

//        foreach (XmlNode child in node.ChildNodes)
//        {
//            if(!ValidateSorted(child))
//            {
//                return false;
//            }
//        }

//        return true;
//    }
//    [Fact]
//    public void TestSortXml()
//    {
//        const string FILENAME = "../../../../test_files/doraemon.SC2Bank";

//        XmlDocument xmlDocument = new XmlDocument();
//        xmlDocument.Load(FILENAME);

//        Assert.NotNull(xmlDocument);

//        var root = xmlDocument.DocumentElement;
//        Assert.NotNull(root);

//        XmlUtils.SortXml(root);
//        //xmlDocument.Save(FILENAME + '2');

//        //check if order of names is sorted in ascending orderE
//        Assert.True(ValidateSorted(root));
//    }
//}

namespace BankSorter.Tests;
using Program;
using System;
using System.Reflection.Metadata;
using System.Xml;
using Xunit.Abstractions;

public class TestBankSorter
{
    private readonly ITestOutputHelper testOutputHelper;

    public TestBankSorter(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    private bool ValidateSortedNode(XmlNode node)
    {
        if (node == null)
        {
            return true;
        }

        Func<XmlNode, string> comparator = node => node.Attributes?["name"]?.Value ?? node.Name;
        var indicies = Enumerable.Range(0, node.ChildNodes.Count).OrderBy(i => comparator(node.ChildNodes[i])).ToArray();

        return Enumerable.SequenceEqual(indicies, Enumerable.Range(0, node.ChildNodes.Count));
    }

    private bool ValidateSortedTree(XmlNode node)
    {
        if (!ValidateSortedNode(node))
        {
            return false;
        }

        foreach (XmlNode child in node.ChildNodes)
        {
            if (!ValidateSortedNode(child))
            {
                return false;
            }
        }

        return true;
    }

    [Fact]
    public void TestSortXml()
    {
        const string FILENAME = "../../../../test_files/doraemon.SC2Bank";

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(FILENAME);

        Assert.NotNull(xmlDocument);

        var root = xmlDocument.DocumentElement;
        Assert.NotNull(root);

        XmlUtils.SortXml(root);
        //xmlDocument.Save(FILENAME + '2');

        //check if order of names is sorted in ascending orderE
        Assert.True(ValidateSortedTree(root));
    }

    [Fact]
    public void TestSortElements()
    {
        const string FILENAME = "../../../../test_files/basic.SC2Bank";

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(FILENAME);

        Assert.NotNull(xmlDocument);

        var root = xmlDocument.DocumentElement;
        Assert.NotNull(root);

        Func<XmlNode, string> comparator = node => node.Attributes?["name"]?.Value ?? node.Name;
        XmlUtils.SortElements(root, comparator);
        //xmlDocument.Save(FILENAME + '2');

        //check if order of names is sorted in ascending orderE
        Assert.True(ValidateSortedNode(root));
    }
}