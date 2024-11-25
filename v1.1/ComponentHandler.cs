/*
	DotNet Core XML Test Code: Component Stock
	Version 1.1

	Author: Rafael Sabe
	Email: rafaelmsabe@gmail.com
*/

using System;
using System.Xml;

namespace CompStock;

public static class ComponentHandler
{
	private static XmlDocument componentsXml = new XmlDocument();
	private static XmlElement? componentsRoot = null;
	private static string errorMsg = "";

	public static bool Initialize()
	{
		try
		{
			componentsXml.Load("xml\\components.xml");
		}
		catch(Exception e)
		{
			errorMsg = "Could not locate components.xml";
			return false;
		}

		componentsRoot = componentsXml.DocumentElement;
		if(componentsRoot == null)
		{
			errorMsg = "Root Element is missing. XML file probably corrupted";
			return false;
		}

		if(!componentsRoot.LocalName.Equals("components"))
		{
			errorMsg = "Root Element name doesn't match expected. XML file probably corrupted";
			return false;
		}

		foreach(XmlNode node in componentsRoot.GetElementsByTagName("component"))
		{
			if(!((XmlElement) node).HasAttribute("name"))
			{
				errorMsg = "One or more component nodes is missing critical attribute \"name\". XML file probably corrupted";
				return false;
			}

			if(!((XmlElement) node).HasAttribute("type"))
			{
				errorMsg = "One or more component nodes is missing critical attribute \"type\". XML file probably corrupted";
				return false;
			}

			if(!((XmlElement) node).HasAttribute("text"))
			{
				errorMsg = "One or more component nodes is missing critical attribute \"text\". XML file probably corrupted";
				return false;
			}
		}

		return true;
	}

	public static string GetLastErrorMessage()
	{
		return errorMsg;
	}

	public static int GetComponentCount() //This method returns the number of known components, NOT the amount of components in stock.
	{
		return componentsRoot.GetElementsByTagName("component").Count;
	}

	public static XmlNodeList GetAllComponentNodes()
	{
		return componentsRoot.GetElementsByTagName("component");
	}

	public static string[] GetAllComponentNames()
	{
		XmlNodeList nodes = componentsRoot.GetElementsByTagName("component");
		string[] names = new string[nodes.Count];

		for(int i = 0; i < nodes.Count; i++) names[i] = nodes[i].Attributes["name"].Value;

		return names;
	}

	public static string[] GetAllComponentTexts()
	{
		XmlNodeList nodes = componentsRoot.GetElementsByTagName("component");
		string[] texts = new string[nodes.Count];

		for(int i = 0; i < nodes.Count; i++) texts[i] = nodes[i].Attributes["text"].Value;

		return texts;
	}

	public static XmlNode? GetComponentByName(string name)
	{
		XmlNodeList nodes = componentsRoot.GetElementsByTagName("component");

		foreach(XmlNode node in nodes) if(node.Attributes["name"].Value.Equals(name)) return node;

		return null;
	}

	public static XmlNode? GetComponentByText(string text)
	{
		XmlNodeList nodes = componentsRoot.GetElementsByTagName("component");

		foreach(XmlNode node in nodes) if(node.Attributes["text"].Value.Equals(text)) return node;

		return null;
	}

	public static XmlNodeList? GetComponentListByType(string type)
	{
		XmlNodeList nodes = componentsRoot.GetElementsByTagName((type + "_type"));
		if(nodes.Count < 1) return null;

		XmlElement componentTypeBlock = (XmlElement) nodes[0];

		return componentTypeBlock.ChildNodes;
	}

	public static string[]? GetNameListByType(string type)
	{
		XmlNodeList? nodes = GetComponentListByType(type);
		if(nodes == null) return null;

		string[] names = new string[nodes.Count];

		for(int i = 0; i < nodes.Count; i++) names[i] = nodes[i].Attributes["name"].Value;

		return names;
	}

	public static string[]? GetTextListByType(string type)
	{
		XmlNodeList? nodes = GetComponentListByType(type);
		if(nodes == null) return null;

		string[] texts = new string[nodes.Count];

		for(int i = 0; i < nodes.Count; i++) texts[i] = nodes[i].Attributes["text"].Value;

		return texts;
	}

	public static XmlNode[]? GetComponentListBySubtype(string type, string subtype)
	{
		XmlNodeList? compListType = GetComponentListByType(type);
		List<XmlNode> compListSubtype = new List<XmlNode>();

		if(compListType == null) return null;

		foreach(XmlNode node in compListType)
		{
			if(!((XmlElement) node).HasAttribute("subtype")) continue;
			if(node.Attributes["subtype"].Value.Equals(subtype)) compListSubtype.Add(node);
		}

		if(compListSubtype.Count < 1) return null;

		return compListSubtype.ToArray();
	}

	public static string[]? GetNameListBySubtype(string type, string subtype)
	{
		XmlNode[]? nodes = GetComponentListBySubtype(type, subtype);
		if(nodes == null) return null;

		string[] names = new string[nodes.Length];

		for(int i = 0; i < nodes.Length; i++) names[i] = nodes[i].Attributes["name"].Value;

		return names;
	}

	public static string[]? GetTextListBySubtype(string type, string subtype)
	{
		XmlNode[]? nodes = GetComponentListBySubtype(type, subtype);
		if(nodes == null) return null;

		string[] texts = new string[nodes.Length];

		for(int i = 0; i < nodes.Length; i++) texts[i] = nodes[i].Attributes["text"].Value;

		return texts;
	}

	public static int GetAttributeCount(XmlNode? component)
	{
		if(component == null) return -1;

		return component.Attributes.Count;
	}

	public static string[]? GetAllAttributeNames(XmlNode? component)
	{
		if(component == null) return null;

		string[] names = new string[component.Attributes.Count];

		for(int i = 0; i < component.Attributes.Count; i++) names[i] = component.Attributes[i].LocalName;

		return names;
	}

	public static string? GetAttributeValue(XmlNode? component, string attributeName)
	{
		if(component == null) return null;
		if(!((XmlElement) component).HasAttribute(attributeName)) return null;

		return component.Attributes[attributeName].Value;
	}

	public static string? GetAttributeValue(XmlNode? component, int index)
	{
		if(component == null) return null;
		if(index < 0) return null;
		if(component.Attributes[index] == null) return null;

		return component.Attributes[index].Value;
	}

	public static string? GetComponentName(XmlNode? component)
	{
		if(component == null) return null;

		return component.Attributes["name"].Value;
	}

	public static string? GetComponentType(XmlNode? component)
	{
		if(component == null) return null;

		return component.Attributes["type"].Value;
	}

	public static string? GetComponentText(XmlNode? component)
	{
		if(component == null) return null;

		return component.Attributes["text"].Value;
	}

	public static string? GetComponentSubtype(XmlNode? component)
	{
		if(component == null) return null;
		if(!((XmlElement) component).HasAttribute("subtype")) return "Not Applicable";

		return component.Attributes["subtype"].Value;
	}
}
