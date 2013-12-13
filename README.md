# WP Sidebar

A simple sidebar control which is commonly used in Android apps.

Currently this control is available for Windows Phone 8 apps only. I do not plan to add a Windows Phone 7 version, but feel free to create one. This project is compatible with Visual Studio 2013.

# How to use

1. Get the control - I recommend using NuGet: https://www.nuget.org/packages/SidebarWP8
2. Import the namespace in your XAML: `xmlns:sidebar="clr-namespace:SidebarWP8;assembly=SidebarWP8"`
3. Include the control in the content area of your page: `<sidebar:SidebarControl />`
4. Customise the control and its content:
	- `HeaderText`:  Set the header text
	- `HeaderTemplate`: If you want to create a more fancy header area, create your own template
	- `HeaderBackground`: Set the background of the header area
	- `HeaderForeground`: Set the foreground of the header area
	- `SidebarBackground`: Set the background of the sidebar
5. Provide contents:
	- Put the stuff to present in the sidebar inside the Property `SidebarContent`: `<sidebar:SidebarControl.SidebarContent><Grid>...</Grid></sidebar:SidebarControl.SidebarContent>`
	- Put the pages content in the content area of the control
	
You may take a look at the sample project which is included in the repository.

# Contribute
Feel free to contribute to this project: add issues or create pull request.

# Changelog
## 1.0.0:
* Initial implementation