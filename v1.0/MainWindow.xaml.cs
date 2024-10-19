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

namespace CompStock;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		this.InitializeComponent();

		if(!ComponentHandler.Initialize())
		{
			MessageBox.Show(("Error: " + ComponentHandler.GetLastErrorMessage()), "Error on components.xml", MessageBoxButton.OK, MessageBoxImage.Error);
			Application.Current.Shutdown(1);
		}

		if(!CircuitHandler.Initialize())
		{
			MessageBox.Show(("Error: " + CircuitHandler.GetLastErrorMessage()), "Error on circuits.xml", MessageBoxButton.OK, MessageBoxImage.Error);
			Application.Current.Shutdown(1);
		}

		if(!StockHandler.Initialize())
		{
			MessageBox.Show(("Error: " + StockHandler.GetLastErrorMessage()), "Error on stock.xml", MessageBoxButton.OK, MessageBoxImage.Error);
			Application.Current.Shutdown(1);
		}

		RuntimeHandler.Status = RuntimeStatus.MAIN;
		this.GoToMainPage();
	}

	public void GoToMainPage()
	{
		this.Content = new MainPage();
	}

	public void GoToListPage()
	{
		this.Content = new ListPage();
	}

	public void GoToCompDetailPage()
	{
		this.Content = new CompDetailPage();
	}
}
