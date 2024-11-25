/*
	DotNet Core XML Test Code: Component Stock
	Version 1.1

	Author: Rafael Sabe
	Email: rafaelmsabe@gmail.com
*/

using System;

namespace CompStock;

public static class RuntimeHandler
{
	public static RuntimeStatus Status = RuntimeStatus.MAIN;
	public static string[]? ListPageArgs = null;
	public static string[]? ListPageListTexts = null;
	public static string CompDetailPageText = "";
}

public enum RuntimeStatus
{
	MAIN,
	LIST_KNOWN_COMPONENTS,
	LIST_KNOWN_CIRCUITS,
	LIST_STOCK,
	LIST_AVAILABLE_CIRCUITS,
	LIST_BUY_COMPONENTS,
	LIST_USE_COMPONENTS,
	LIST_BUY_CIRCUIT_COMPONENTS,
	LIST_BUILD_CIRCUIT,
	LIST_CIRCUIT_REQUIREMENTS,
	VIEW_COMPONENT_DETAILS
}
