using Microsoft.Phone.Controls;
using System.ComponentModel;

namespace SidebarWP8.Test
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Important
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (sidebarControl.IsOpen)
            {
                sidebarControl.IsOpen = false;
                e.Cancel = true;
            }
        }
    }
}