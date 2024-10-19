/*
	DotNet Core XML Test Code: Component Stock

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

public static class StockHandler
{
	private static XmlDocument stockXml = new XmlDocument();
	private static XmlElement? stockRoot = null;
	private static string errorMsg = "";
	private const string stockXmlPath = "xml\\stock.xml";

	private static void createStockXml()
	{
		XmlWriter xmldoc = XmlWriter.Create(stockXmlPath);
		xmldoc.WriteStartElement("stock");
		xmldoc.WriteFullEndElement();
		xmldoc.Flush();
		xmldoc.Close();
	}

	private static bool openStockXml()
	{
		try
		{
			stockXml.Load(stockXmlPath);
		}
		catch(Exception e)
		{
			return false;
		}

		return true;
	}

	private static bool initStockXml()
	{
		if(openStockXml()) return true;

		createStockXml();
		return openStockXml();
	}

	public static bool Initialize()
	{
		int n = 0;

		if(!initStockXml())
		{
			errorMsg = "Could not load stock.xml\r\n";
			return false;
		}

		stockRoot = stockXml.DocumentElement;

		if(stockRoot == null)
		{
			errorMsg = "Root Element is missing. XML file probably corrupted";
			return false;
		}

		if(!stockRoot.LocalName.Equals("stock"))
		{
			errorMsg = "Root Element name doesn't match expected. XML file probably corrupted";
			return false;
		}

		foreach(XmlNode node in stockRoot.ChildNodes)
		{
			if(!node.LocalName.Equals("comp"))
			{
				errorMsg = "Unexpected node name found. XML file probably corrupted";
				return false;
			}

			if(!((XmlElement) node).HasAttribute("count"))
			{
				errorMsg = "One or more comp nodes is missing critical attribute. XML file probably corrupted";
				return false;
			}

			if(node.InnerXml.Length < 1)
			{
				errorMsg = "One or more comp nodes is missing inner XML. XML file probably corrupted";
				return false;
			}

			try
			{
				n = Int32.Parse(node.Attributes["count"].Value);
			}
			catch(Exception e)
			{
				errorMsg = "One or more comp nodes has an invalid value for attribute \"count\". XML file probably corrupted";
				return false;
			}

			if(n < 1)
			{
				errorMsg = "One or more comp nodes has an invalid value for attribute \"count\". XML file probably corrupted";
				return false;
			}
		}

		return true;
	}

	public static string GetLastErrorMessage()
	{
		return errorMsg;
	}

	//WARNING: 
	//1. Comp is not the same as Component. Although they represent the same thing, they have different node structures.
	//2. Comp is not the same as Comp in CircuitHandler class. They represent the same thing, but have different node structures.

	public static int GetCompNodeCount()
	{
		return stockRoot.ChildNodes.Count;
	}

	public static XmlNodeList? GetAllCompNodes()
	{
		if(stockRoot.ChildNodes.Count < 1) return null;

		return stockRoot.ChildNodes;
	}

	public static XmlNode? GetCompByName(string name)
	{
		if(name.Length < 1) return null;

		XmlNodeList nodes = stockRoot.ChildNodes;

		foreach(XmlNode node in nodes) if(node.InnerXml.Equals(name)) return node;

		return null;
	}

	public static XmlNode? GetCompByIndex(int index)
	{
		if(index < 0) return null;
		if(index >= stockRoot.ChildNodes.Count) return null;

		return stockRoot.ChildNodes[index];
	}

	//Only for already existing nodes
	public static bool AddComp(XmlNode? comp, int count)
	{
		if(comp == null) return false;
		if(count < 1) return false;

		int currCount = Int32.Parse(comp.Attributes["count"].Value);
		currCount += count;

		comp.Attributes["count"].Value = currCount.ToString();

		stockXml.Save(stockXmlPath);

		return true;
	}

	//Will create a new node if said component does exist on stock
	public static bool AddComp(string name, int count)
	{
		if(name.Length < 1 || count < 1) return false;

		XmlNode? comp = GetCompByName(name);

		if(comp == null)
		{
			stockRoot.AppendChild(stockXml.CreateElement("comp"));
			stockRoot.LastChild.InnerXml = name;
			((XmlElement) stockRoot.LastChild).SetAttribute("count", "0");
			comp = stockRoot.LastChild;
		}

		return AddComp(comp, count);
	}

	//Only for already existing nodes
	public static bool AddComp(int index, int count)
	{
		XmlNode? comp = GetCompByIndex(index);
		if(comp == null) return false;
		if(count < 1) return false;

		return AddComp(comp, count);
	}

	public static bool RemoveComp(XmlNode? comp, int count)
	{
		if(comp == null) return false;
		if(count < 1) return false;

		int currCount = Int32.Parse(comp.Attributes["count"].Value);

		if(currCount < count) return false;

		currCount -= count;

		if(currCount <= 0) stockRoot.RemoveChild(comp);
		else comp.Attributes["count"].Value = currCount.ToString();

		stockXml.Save(stockXmlPath);

		return true;
	}

	public static bool RemoveComp(string name, int count)
	{
		if(name.Length < 1 || count < 1) return false;

		XmlNode? comp = GetCompByName(name);

		if(comp == null) return false;

		return RemoveComp(comp, count);
	}

	public static bool RemoveComp(int index, int count)
	{
		XmlNode? comp = GetCompByIndex(index);

		if(comp == null) return false;
		if(count < 1) return false;

		return RemoveComp(comp, count);
	}

	public static bool HasComponent(string name, int count)
	{
		int stockCount = 0;
		XmlNodeList nodes = stockRoot.ChildNodes;

		foreach(XmlNode node in nodes)
		{
			if(node.InnerXml.Equals(name))
			{
				stockCount = Int32.Parse(node.Attributes["count"].Value);
				break;
			}
		}

		return (stockCount >= count);
	}

	public static bool HasType(string type, int count)
	{
		int stockCount = 0;
		XmlNode? component = null;
		XmlNodeList nodes = stockRoot.ChildNodes;

		foreach(XmlNode node in nodes)
		{
			component = ComponentHandler.GetComponentByName(node.InnerXml);
			if(component == null) continue;

			if(component.Attributes["type"].Value.Equals(type))
			{
				stockCount = Int32.Parse(node.Attributes["count"].Value);
				break;
			}
		}

		return (stockCount >= count);
	}

	public static bool HasSubtype(string type, string subtype, int count)
	{
		int stockCount = 0;
		XmlNode? component = null;
		XmlNodeList nodes = stockRoot.ChildNodes;

		foreach(XmlNode node in nodes)
		{
			component = ComponentHandler.GetComponentByName(node.InnerXml);
			if(component == null) continue;
			if(!((XmlElement) component).HasAttribute("subtype")) continue;

			if(component.Attributes["type"].Value.Equals(type) && component.Attributes["subtype"].Value.Equals(subtype))
			{
				stockCount = Int32.Parse(node.Attributes["count"].Value);
				break;
			}
		}

		return (stockCount >= count);
	}

	//Returns the first node in stock that matches the input name
	public static XmlNode? GetNextCompByName(string name)
	{
		XmlNodeList nodes = stockRoot.ChildNodes;

		foreach(XmlNode node in nodes) if(node.InnerXml.Equals(name)) return node;

		return null;
	}

	//Returns the first node in stock whose component matches the input type
	public static XmlNode? GetNextCompByType(string type)
	{
		XmlNode? component = null;
		XmlNodeList nodes = stockRoot.ChildNodes;

		foreach(XmlNode node in nodes)
		{
			component = ComponentHandler.GetComponentByName(node.InnerXml);
			if(component == null) continue;

			if(component.Attributes["type"].Value.Equals(type)) return node;
		}

		return null;
	}

	//Returns the first node in stock whose component matches the input type and subtype
	public static XmlNode? GetNextCompBySubtype(string type, string subtype)
	{
		XmlNode? component = null;
		XmlNodeList nodes = stockRoot.ChildNodes;

		foreach(XmlNode node in nodes)
		{
			component = ComponentHandler.GetComponentByName(node.InnerXml);
			if(component == null) continue;
			if(!((XmlElement) component).HasAttribute("subtype")) continue;

			if(component.Attributes["type"].Value.Equals(type) && component.Attributes["subtype"].Value.Equals(subtype)) return node;
		}

		return null;
	}

	public static bool HasAllCircuitComps(XmlNode? circuit)
	{
		if(circuit == null) return false;

		int count = 0;
		XmlNodeList? circuitComps = CircuitHandler.GetCircuitComps(circuit);

		foreach(XmlNode node in circuitComps)
		{
			count = Int32.Parse(node.Attributes["count"].Value);

			if(((XmlElement) node).HasAttribute("name"))
			{
				if(!HasComponent(node.Attributes["name"].Value, count)) return false;
			}

			if(((XmlElement) node).HasAttribute("type"))
			{
				if(((XmlElement) node).HasAttribute("subtype"))
				{
					if(!HasSubtype(node.Attributes["type"].Value, node.Attributes["subtype"].Value, count)) return false;
				}
				else
				{
					if(!HasType(node.Attributes["type"].Value, count)) return false;
				}
			}
		}

		return true;
	}

	public static XmlNode[]? GetAvailableCircuits()
	{
		List<XmlNode> available = new List<XmlNode>();

		foreach(XmlNode circuit in CircuitHandler.GetAllCircuitNodes()) if(HasAllCircuitComps(circuit)) available.Add(circuit);

		if(available.Count < 1) return null;

		return available.ToArray();
	}

	//If circuit component is generic type or subtype, the component to be added will be the first element returned by ComponentHandler.GetNameListByType()/ComponentHandler.GetNameListBySubtype()
	public static bool BuyCompsForCircuit(XmlNode? circuit)
	{
		if(circuit == null) return false;

		int count = 0;
		string[]? names = null;
		XmlNodeList? circuitComps = CircuitHandler.GetCircuitComps(circuit);

		foreach(XmlNode node in circuitComps)
		{
			count = Int32.Parse(node.Attributes["count"].Value);

			if(((XmlElement) node).HasAttribute("name")) AddComp(node.Attributes["name"].Value, count);
			else if(((XmlElement) node).HasAttribute("type"))
			{
				if(((XmlElement) node).HasAttribute("subtype"))
				{
					names = ComponentHandler.GetNameListBySubtype(node.Attributes["type"].Value, node.Attributes["subtype"].Value);
					if(names == null) return false;

					AddComp(names[0], count);
				}
				else
				{
					names = ComponentHandler.GetNameListByType(node.Attributes["type"].Value);
					if(names == null) return false;

					AddComp(names[0], count);
				}
			}
		}

		return true;
	}

	//If circuit component is generic type or subtype, the component to be removed will be the first stock element that matches ComponentHandler.GetNameListByType()/ComponentHandler.GetNameListBySubtype()
	public static bool UseCompsForCircuit(XmlNode? circuit)
	{
		if(circuit == null) return false;
		if(!HasAllCircuitComps(circuit)) return false;

		int count = 0;
		int ncomp = 0;
		XmlNodeList circuitComps = CircuitHandler.GetCircuitComps(circuit);

		foreach(XmlNode node in circuitComps)
		{
			count = Int32.Parse(node.Attributes["count"].Value);

			if(((XmlElement) node).HasAttribute("name")) RemoveComp(node.Attributes["name"].Value, count);
			else if(((XmlElement) node).HasAttribute("type"))
			{
				if(((XmlElement) node).HasAttribute("subtype"))
				{
					for(ncomp = 0; ncomp < count; ncomp++) RemoveComp(GetNextCompBySubtype(node.Attributes["type"].Value, node.Attributes["subtype"].Value), 1);
				}
				else
				{
					for(ncomp = 0; ncomp < count; ncomp++) RemoveComp(GetNextCompByType(node.Attributes["type"].Value), 1);
				}
			}
		}

		return true;
	}

	//Format Unused
	/*public static string ListStock()
	{
		int count = 0;
		XmlNode? component = null;
		string output = "Component Stock:\r\n\r\n";

		foreach(XmlNode node in stockRoot.ChildNodes)
		{
			component = ComponentHandler.GetComponentByName(node.InnerXml);
			if(component == null) continue;

			count = Int32.Parse(node.Attributes["count"].Value);

			output += component.Attributes["text"].Value;
			output += ": ";
			output += count.ToString();
			output += "\r\n";
		}

		return output;
	}*/

	public static string[]? ListStock()
	{
		if(stockRoot.ChildNodes.Count < 1) return null;

		int count = 0;
		XmlNode? component = null;

		XmlNodeList nodes = stockRoot.ChildNodes;
		string[] stockComps = new string[nodes.Count];

		for(int i = 0; i < nodes.Count; i++)
		{
			component = ComponentHandler.GetComponentByName(nodes[i].InnerXml);
			if(component == null) continue;

			count = Int32.Parse(nodes[i].Attributes["count"].Value);

			stockComps[i] = component.Attributes["text"].Value;
			stockComps[i] += ": ";
			stockComps[i] += count.ToString();
		}

		return stockComps;
	}

	//Format Unused
	/*public static string ListAvailableCircuits()
	{
		XmlNode[]? availableCircuits = GetAvailableCircuits();

		if(availableCircuits == null) return "There are no circuits available for now.";

		string output = "Available Circuits:\r\n\r\n";

		foreach(XmlNode circuit in availableCircuits)
		{
			output += circuit.Attributes["text"].Value;
			output += "\r\n";
		}

		return output;
	}*/

	public static string[]? ListAvailableCircuits()
	{
		XmlNode[]? availableCircuits = GetAvailableCircuits();

		if(availableCircuits == null) return null;

		string[] circuits = new string[availableCircuits.Length];

		for(int i = 0; i < availableCircuits.Length; i++) circuits[i] = availableCircuits[i].Attributes["text"].Value;

		return circuits;
	}
}
