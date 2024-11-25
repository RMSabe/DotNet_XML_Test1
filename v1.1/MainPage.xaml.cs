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

using System.Diagnostics;

namespace CompStock;

public partial class MainPage : Page
{
	private MainWindow parent = (MainWindow) Application.Current.MainWindow;

	public MainPage()
	{
		this.InitializeComponent();
		this.Width = this.parent.Width;
		this.Height = this.parent.Height;
	}

	private void onButton1Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_KNOWN_COMPONENTS;
		this.parent.GoToListPage();
	}

	private void onButton2Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_KNOWN_CIRCUITS;
		this.parent.GoToListPage();
	}

	private void onButton3Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_STOCK;
		this.parent.GoToListPage();
	}

	private void onButton4Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_AVAILABLE_CIRCUITS;
		this.parent.GoToListPage();
	}

	private void onButton5Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_BUY_COMPONENTS;
		this.parent.GoToListPage();
	}

	private void onButton6Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_USE_COMPONENTS;
		this.parent.GoToListPage();
	}

	private void onButton7Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_BUY_CIRCUIT_COMPONENTS;
		this.parent.GoToListPage();
	}

	private void onButton8Clicked(object sender, RoutedEventArgs e)
	{
		RuntimeHandler.Status = RuntimeStatus.LIST_BUILD_CIRCUIT;
		this.parent.GoToListPage();
	}
}
