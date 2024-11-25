/*
	DotNet Core XML Test Code: Component Stock
	Version 1.1

	Author: Rafael Sabe
	Email: rafaelmsabe@gmail.com
*/

using System;
using System.Xml;

namespace CompStock;

public static class CircuitHandler
{
	private static XmlDocument circuitsXml = new XmlDocument();
	private static XmlElement? circuitsRoot = null;
	private static string errorMsg = "";

	public static bool Initialize()
	{
		try
		{
			circuitsXml.Load("xml\\circuits.xml");
		}
		catch(Exception e)
		{
			errorMsg = "Could not locate circuits.xml";
			return false;
		}

		circuitsRoot = circuitsXml.DocumentElement;
		if(circuitsRoot == null)
		{
			errorMsg = "Root Element is missing. XML file probably corrupted";
			return false;
		}

		if(!circuitsRoot.LocalName.Equals("circuits"))
		{
			errorMsg = "Root Element name doesn't match expected. XML file probably corrupted";
			return false;
		}

		foreach(XmlNode node in circuitsRoot.GetElementsByTagName("circuit"))
		{
			if(!((XmlElement) node).HasAttribute("name"))
			{
				errorMsg = "One or more circuit nodes is missing critical attribute \"name\". XML file probably corrupted";
				return false;
			}

			if(!((XmlElement) node).HasAttribute("text"))
			{
				errorMsg = "One or more circuit nodes is missing critical attribute \"text\". XML file probably corrupted";
				return false;
			}
		}

		foreach(XmlNode circuit in circuitsRoot.GetElementsByTagName("circuit"))
		{
			foreach(XmlNode component in ((XmlElement) circuit).ChildNodes)
			{
				if(!((XmlElement) component).HasAttribute("count"))
				{
					errorMsg = "Circuit \"" + circuit.Attributes["name"].Value + "\": One or more component nodes is missing critical attribute. XML file probably corrupted";
					return false;
				}

				if((!((XmlElement) component).HasAttribute("name")) && (!((XmlElement) component).HasAttribute("type")))
				{
					errorMsg = "Circuit \"" + circuit.Attributes["name"].Value + "\": One or more component nodes is missing critical attribute. XML file probably corrupted";
					return false;
				}
			}
		}

		return true;
	}

	public static string GetLastErrorMessage()
	{
		return errorMsg;
	}

	public static int GetCircuitCount()
	{
		return circuitsRoot.GetElementsByTagName("circuit").Count;
	}

	public static XmlNodeList GetAllCircuitNodes()
	{
		return circuitsRoot.GetElementsByTagName("circuit");
	}

	public static string[] GetAllCircuitNames()
	{
		XmlNodeList nodes = circuitsRoot.GetElementsByTagName("circuit");

		string[] names = new string[nodes.Count];

		for(int i = 0; i < nodes.Count; i++) names[i] = nodes[i].Attributes["name"].Value;

		return names;
	}

	public static string[] GetAllCircuitTexts()
	{
		XmlNodeList nodes = circuitsRoot.GetElementsByTagName("circuit");

		string[] texts = new string[nodes.Count];

		for(int i = 0; i < nodes.Count; i++) texts[i] = nodes[i].Attributes["text"].Value;

		return texts;
	}

	public static XmlNode? GetCircuitByName(string name)
	{
		XmlNodeList nodes = circuitsRoot.GetElementsByTagName("circuit");

		foreach(XmlNode node in nodes) if(node.Attributes["name"].Value.Equals(name)) return node;

		return null;
	}

	public static XmlNode? GetCircuitByText(string text)
	{
		XmlNodeList nodes = circuitsRoot.GetElementsByTagName("circuit");

		foreach(XmlNode node in nodes) if(node.Attributes["text"].Value.Equals(text)) return node;

		return null;
	}

	public static XmlNode? GetCircuitByIndex(int index)
	{
		if(index < 0) return null;

		XmlNodeList nodes = circuitsRoot.GetElementsByTagName("circuit");

		if(index >= nodes.Count) return null;

		return nodes[index];
	}

	//WARNING: 
	//1. Comp is not the same as Component. Although they represent the same thing, they have different node structures.
	//2. A Comp node might be a generic component type instead of a specific component, which means it will not have a component node equivalent.

	public static XmlNodeList? GetCircuitComps(XmlNode? circuit)
	{
		if(circuit == null) return null;

		return ((XmlElement) circuit).ChildNodes;
	}

	public static string ListCircuits()
	{
		string output = "Known Circuits:\r\n\r\n";

		foreach(string circuitText in GetAllCircuitTexts())
		{
			output += circuitText;
			output += "\r\n";
		}

		return output;
	}

	//Format Unused
	/*public static string ListCircuitComps(XmlNode? circuit)
	{
		if(circuit == null) return "";

		int count = 0;
		XmlNode? component = null;

		string output = circuit.Attributes["text"].Value;
		output += " required components:\r\n\r\n";

		foreach(XmlNode node in GetCircuitComps(circuit))
		{
			count = Int32.Parse(node.Attributes["count"].Value);

			if(((XmlElement) node).HasAttribute("name"))
			{
				component = ComponentHandler.GetComponentByName(node.Attributes["name"].Value);
				if(component != null)
				{
					output += component.Attributes["text"].Value;
					output += ": ";
					output += count.ToString();
					output += "\r\n";
				}

				continue;
			}

			if(((XmlElement) node).HasAttribute("type"))
			{
				if(((XmlElement) node).HasAttribute("subtype"))
				{
					output += "<Generic> ";
					output += node.Attributes["type"].Value + " ";
					output += node.Attributes["subtype"].Value + ": ";
					output += count.ToString() + "\r\n";
				}
				else
				{
					output += "<Generic> ";
					output += node.Attributes["type"].Value + ": ";
					output += count.ToString() + "\r\n";
				}
			}
		}

		return output;
	}*/

	public static string[]? ListCircuitComps(XmlNode? circuit)
	{
		if(circuit == null) return null;

		int count = 0;
		XmlNode? component = null;

		XmlNodeList? nodes = GetCircuitComps(circuit);
		string[] compList = new string[nodes.Count];

		for(int i = 0; i < nodes.Count; i++)
		{
			count = Int32.Parse(nodes[i].Attributes["count"].Value);

			if(((XmlElement) nodes[i]).HasAttribute("name"))
			{
				component = ComponentHandler.GetComponentByName(nodes[i].Attributes["name"].Value);
				if(component != null)
				{
					compList[i] = component.Attributes["text"].Value;
					compList[i] += ": ";
					compList[i] += count.ToString();
				}

				continue;
			}

			if(((XmlElement) nodes[i]).HasAttribute("type"))
			{
				if(((XmlElement) nodes[i]).HasAttribute("subtype"))
				{
					compList[i] = "<Generic> ";
					compList[i] += (nodes[i].Attributes["type"].Value + " ");
					compList[i] += (nodes[i].Attributes["subtype"].Value + ": ");
					compList[i] += count.ToString();
				}
				else
				{
					compList[i] = "<Generic> ";
					compList[i] += (nodes[i].Attributes["type"].Value + ": ");
					compList[i] += count.ToString();
				}
			}
		}

		return compList;
	}
}
