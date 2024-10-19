/*
	DotNet Core XML Test Code: Component Stock

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

public partial class MainPage : Page
{
	private MainWindow parent = (MainWindow) Application.Current.MainWindow;

	public MainPage()
	{
		this.InitializeComponent();
		this.Width = parent.Width;
		this.Height = parent.Height;
	}

	private void onButton1Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_KNOWN_COMPONENTS;
		parent.GoToListPage();
	}

	private void onButton2Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_KNOWN_CIRCUITS;
		parent.GoToListPage();
	}

	private void onButton3Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_STOCK;
		parent.GoToListPage();
	}

	private void onButton4Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_AVAILABLE_CIRCUITS;
		parent.GoToListPage();
	}

	private void onButton5Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_BUY_COMPONENTS;
		parent.GoToListPage();
	}

	private void onButton6Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_USE_COMPONENTS;
		parent.GoToListPage();
	}

	private void onButton7Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_BUY_CIRCUIT_COMPONENTS;
		parent.GoToListPage();
	}

	private void onButton8Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_BUILD_CIRCUIT;
		parent.GoToListPage();
	}
}

public partial class ListPage : Page
{
	private MainWindow parent = (MainWindow) Application.Current.MainWindow;

	public ListPage()
	{
		this.InitializeComponent();
		this.Width = parent.Width;
		this.Height = parent.Height;

		this.loadPage();
	}

	private void loadPage()
	{
		switch(RuntimeHandler.Status)
		{
			case RuntimeStatus.LIST_KNOWN_COMPONENTS:
				this._TextBlock2.Visibility = Visibility.Hidden;
				this._TextBox.Visibility = Visibility.Hidden;

				this._TextBlock1.Text = "Known Component List";
				RuntimeHandler.ListPageListTexts = ComponentHandler.GetAllComponentTexts();
				break;

			case RuntimeStatus.LIST_KNOWN_CIRCUITS:
				this._TextBlock2.Visibility = Visibility.Hidden;
				this._TextBox.Visibility = Visibility.Hidden;

				this._TextBlock1.Text = "Known Circuit List";
				RuntimeHandler.ListPageListTexts = CircuitHandler.GetAllCircuitTexts();
				break;

			case RuntimeStatus.LIST_STOCK:
				this._TextBlock2.Visibility = Visibility.Hidden;
				this._TextBox.Visibility = Visibility.Hidden;

				this._TextBlock1.Text = "Stocked Component List";
				RuntimeHandler.ListPageListTexts = StockHandler.ListStock();
				break;

			case RuntimeStatus.LIST_AVAILABLE_CIRCUITS:
				this._TextBlock2.Visibility = Visibility.Hidden;
				this._TextBox.Visibility = Visibility.Hidden;

				this._TextBlock1.Text = "Currently Available Circuits";
				RuntimeHandler.ListPageListTexts = StockHandler.ListAvailableCircuits();
				break;

			case RuntimeStatus.LIST_BUY_COMPONENTS:
				this._TextBlock2.Visibility = Visibility.Visible;
				this._TextBox.Visibility = Visibility.Visible;

				this._TextBox.Text = "";

				this._TextBlock1.Text = "Buy Components";
				RuntimeHandler.ListPageListTexts = ComponentHandler.GetAllComponentTexts();
				break;

			case RuntimeStatus.LIST_USE_COMPONENTS:
				this._TextBlock2.Visibility = Visibility.Visible;
				this._TextBox.Visibility = Visibility.Visible;

				this._TextBox.Text = "";

				this._TextBlock1.Text = "Use Components";
				RuntimeHandler.ListPageListTexts = StockHandler.ListStock();
				break;

			case RuntimeStatus.LIST_BUY_CIRCUIT_COMPONENTS:
				this._TextBlock2.Visibility = Visibility.Hidden;
				this._TextBox.Visibility = Visibility.Hidden;

				this._TextBlock1.Text = "Buy Components For Circuit";
				RuntimeHandler.ListPageListTexts = CircuitHandler.GetAllCircuitTexts();
				break;

			case RuntimeStatus.LIST_BUILD_CIRCUIT:
				this._TextBlock2.Visibility = Visibility.Hidden;
				this._TextBox.Visibility = Visibility.Hidden;

				this._TextBlock1.Text = "Build Circuit";
				RuntimeHandler.ListPageListTexts = StockHandler.ListAvailableCircuits();
				break;

			case RuntimeStatus.LIST_CIRCUIT_REQUIREMENTS:
				this._TextBlock2.Visibility = Visibility.Hidden;
				this._TextBox.Visibility = Visibility.Hidden;

				this._TextBlock1.Text = RuntimeHandler.ListPageArgs[0] + " Required Components";
				RuntimeHandler.ListPageListTexts = CircuitHandler.ListCircuitComps(CircuitHandler.GetCircuitByText(RuntimeHandler.ListPageArgs[0]));
				break;
		}

		this.loadListBox();
	}

	private void loadListBox()
	{
		if(RuntimeHandler.ListPageListTexts == null) return;

		for(int i = 0; i < RuntimeHandler.ListPageListTexts.Length; i++)
		{
			this._ListBox.Items.Add(new ListBoxItem());
			((ListBoxItem) this._ListBox.Items[i]).Name = "_Item_" + i.ToString();
			((ListBoxItem) this._ListBox.Items[i]).Content = RuntimeHandler.ListPageListTexts[i];
		}
	}

	private void onListBoxSelectionChanged(object sender, RoutedEventArgs e)
	{
		if(((ListBox) sender).SelectedItem == null) return;

		ListBoxItem selectedItem = (ListBoxItem) ((ListBox) sender).SelectedItem;
		string content = (string) selectedItem.Content;
		string arg = "";
		int n32 = 0;
		XmlNode? node = null;

		switch(RuntimeHandler.Status)
		{
			case RuntimeStatus.LIST_KNOWN_COMPONENTS:
				RuntimeHandler.CompDetailPageText = content;
				RuntimeHandler.Status = RuntimeStatus.VIEW_COMPONENT_DETAILS;
				parent.GoToCompDetailPage();
				break;

			case RuntimeStatus.LIST_KNOWN_CIRCUITS:
				RuntimeHandler.ListPageArgs = new string[] {content};
				RuntimeHandler.Status = RuntimeStatus.LIST_CIRCUIT_REQUIREMENTS;
				parent.GoToListPage();
				break;

			case RuntimeStatus.LIST_STOCK:
				for(int i = 0; i < content.Length; i++)
				{
					if(content[i] == ':') break;
					arg += content[i];
				}

				RuntimeHandler.CompDetailPageText = arg;
				RuntimeHandler.Status = RuntimeStatus.VIEW_COMPONENT_DETAILS;
				parent.GoToCompDetailPage();
				break;

			case RuntimeStatus.LIST_AVAILABLE_CIRCUITS:
				RuntimeHandler.ListPageArgs = new string[] {content};
				RuntimeHandler.Status = RuntimeStatus.LIST_CIRCUIT_REQUIREMENTS;
				parent.GoToListPage();
				break;

			case RuntimeStatus.LIST_BUY_COMPONENTS:
				if(this._TextBox.Text.Length < 1)
				{
					MessageBox.Show("Quantity must be specified", "Error: No Quantity", MessageBoxButton.OK);
					this._ListBox.UnselectAll();
					return;
				}

				try
				{
					n32 = Int32.Parse(this._TextBox.Text);
				}
				catch(Exception ex)
				{
					MessageBox.Show("Invalid quantity entered", "Error: Invalid Quantity", MessageBoxButton.OK);
					this._ListBox.UnselectAll();
					return;
				}

				if(n32 < 1)
				{
					MessageBox.Show("Invalid quantity entered", "Error: Invalid Quantity", MessageBoxButton.OK);
					this._ListBox.UnselectAll();
					return;
				}

				node = ComponentHandler.GetComponentByText(content);
				if(node == null)
				{
					MessageBox.Show("Something went wrong", "Error: No Component", MessageBoxButton.OK, MessageBoxImage.Error);
					this._ListBox.UnselectAll();
					return;
				}

				if(!StockHandler.AddComp(node.Attributes["name"].Value, n32))
				{
					MessageBox.Show("Something went wrong", "Error: Stock Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
					this._ListBox.UnselectAll();
					return;
				}

				MessageBox.Show(("Successfully added " + n32.ToString() + " " + content + " to the stock"), "Operation Successful", MessageBoxButton.OK);
				RuntimeHandler.Status = RuntimeStatus.MAIN;
				parent.GoToMainPage();
				break;

			case RuntimeStatus.LIST_USE_COMPONENTS:
				if(this._TextBox.Text.Length < 1)
				{
					MessageBox.Show("Quantity must be specified", "Error: No Quantity", MessageBoxButton.OK);
					this._ListBox.UnselectAll();
					return;
				}

				try
				{
					n32 = Int32.Parse(this._TextBox.Text);
				}
				catch(Exception ex)
				{
					MessageBox.Show("Invalid quantity entered", "Error: Invalid Quantity", MessageBoxButton.OK);
					this._ListBox.UnselectAll();
					return;
				}

				if(n32 < 1)
				{
					MessageBox.Show("Invalid quantity entered", "Error: Invalid Quantity", MessageBoxButton.OK);
					this._ListBox.UnselectAll();
					return;
				}

				for(int i = 0; i < content.Length; i++)
				{
					if(content[i] == ':') break;
					arg += content[i];
				}

				node = ComponentHandler.GetComponentByText(arg);
				if(node == null)
				{
					MessageBox.Show("Something went wrong", "Error: No Component", MessageBoxButton.OK, MessageBoxImage.Error);
					this._ListBox.UnselectAll();
					return;
				}

				if(!StockHandler.RemoveComp(node.Attributes["name"].Value, n32))
				{
					MessageBox.Show("Stock does not have requested quantity of the component", "Error: Not Enough Units", MessageBoxButton.OK);
					this._ListBox.UnselectAll();
					return;
				}

				MessageBox.Show(("Successfully removed " + n32.ToString() + " " + arg + " from the stock"), "Operation Successful", MessageBoxButton.OK);
				RuntimeHandler.Status = RuntimeStatus.MAIN;
				parent.GoToMainPage();
				break;

			case RuntimeStatus.LIST_BUY_CIRCUIT_COMPONENTS:
				if(!StockHandler.BuyCompsForCircuit(CircuitHandler.GetCircuitByText(content)))
				{
					MessageBox.Show("Something went wrong", "Error: Unknown", MessageBoxButton.OK, MessageBoxImage.Error);
					this._ListBox.UnselectAll();
					return;
				}

				MessageBox.Show(("Successfully added components for circuit " + content + " to the stock"), "Operation Successful", MessageBoxButton.OK);
				RuntimeHandler.Status = RuntimeStatus.MAIN;
				parent.GoToMainPage();
				break;

			case RuntimeStatus.LIST_BUILD_CIRCUIT:
				if(!StockHandler.UseCompsForCircuit(CircuitHandler.GetCircuitByText(content)))
				{
					MessageBox.Show("Something went wrong", "Error: Unknown", MessageBoxButton.OK, MessageBoxImage.Error);
					this._ListBox.UnselectAll();
					return;
				}

				MessageBox.Show(("Successfully removed components for circuit " + content + " from the stock"), "Operation Successful", MessageBoxButton.OK);
				RuntimeHandler.Status = RuntimeStatus.MAIN;
				parent.GoToMainPage();
				break;

			case RuntimeStatus.LIST_CIRCUIT_REQUIREMENTS:
				if(content[0] == '<') return;

				for(int i = 0; i < content.Length; i++)
				{
					if(content[i] == ':') break;
					arg += content[i];
				}

				RuntimeHandler.CompDetailPageText = arg;
				RuntimeHandler.Status = RuntimeStatus.VIEW_COMPONENT_DETAILS;
				parent.GoToCompDetailPage();
				break;
		}
	}

	private void onButton1Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.MAIN;
		parent.GoToMainPage();
	}
}

public partial class CompDetailPage : Page
{
	private MainWindow parent = (MainWindow) Application.Current.MainWindow;

	public CompDetailPage()
	{
		this.InitializeComponent();
		this.Width = parent.Width;
		this.Height = parent.Height;

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
		{
			this._Button2.Visibility = Visibility.Visible;
			this._Button2.IsEnabled = true;

			textout += ("Datasheet URL: " + component.Attributes["urldatasheet"].Value + "\r\n");
		}
		else
		{
			this._Button2.Visibility = Visibility.Hidden;
			this._Button2.IsEnabled = false;
		}

		this._TextBlock2.Text = textout;
	}

	public void onButton1Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.MAIN;
		parent.GoToMainPage();
	}

	public void onButton2Clicked(object sender, RoutedEventArgs e)
	{
		string? datasheeturl = ComponentHandler.GetAttributeValue(ComponentHandler.GetComponentByText(RuntimeHandler.CompDetailPageText), "urldatasheet");
		if(datasheeturl == null)
		{
			MessageBox.Show("Datasheet URL is missing", "Error: Missing URL", MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		try
		{
			Process.Start(datasheeturl);
		}
		catch(Exception ex)
		{
			MessageBox.Show("Could not load datasheet page to browser", "Error: Browser", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
