/*
	DotNet Core XML Test Code: Component Stock
	Version 1.1

	Author: Rafael Sabe
	Email: rafaelmsabe@gmail.com
*/

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Xml;
using System.Diagnostics;

namespace CompStock;

public partial class CompDetailPage : Page
{
	private MainWindow parent = (MainWindow) Application.Current.MainWindow;

	public CompDetailPage()
	{
		this.InitializeComponent();
		this.Width = this.parent.Width;
		this.Height = this.parent.Height;

		this._TextBlock1.Text = RuntimeHandler.CompDetailPageText + " Details";
		this.loadDetails();
	}

	private void loadDetails()
	{
		XmlElement? component = null;
		string textout = "";

		component = (XmlElement) ComponentHandler.GetComponentByText(RuntimeHandler.CompDetailPageText);
		if(component == null) return;

		textout += ("Name: " + component.Attributes["text"].Value + "\r\n");
		textout += ("Internal name reference: " + component.Attributes["name"].Value + "\r\n");
		textout += ("Component type: " + component.Attributes["type"].Value + "\r\n");

		if(component.HasAttribute("subtype"))
			textout += ("Component variation: " + component.Attributes["subtype"].Value + "\r\n");

		if(component.HasAttribute("vbreakdown"))
			textout += ("Breakdown Voltage: " + component.Attributes["vbreakdown"].Value + "\r\n");

		if(component.HasAttribute("color"))
			textout += ("Color: " + component.Attributes["color"].Value + "\r\n");

		if(component.HasAttribute("polarity"))
			textout += ("Polarity: " + component.Attributes["polarity"].Value + "\r\n");

		if(component.HasAttribute("prefix"))
			textout += ("Prefix: " + component.Attributes["prefix"].Value + "\r\n");
		else
			textout += ("Prefix: " + component.ParentNode.Attributes["prefix"].Value + "\r\n");

		if(component.HasAttribute("urldatasheet"))
			textout += ("Datasheet URL: " + component.Attributes["urldatasheet"].Value + "\r\n");

		this._TextBlock2.Text = textout;
	}

	public void onButton1Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.MAIN;
		this.parent.GoToMainPage();
	}
}
