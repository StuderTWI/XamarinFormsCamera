using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Camera_Test
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
            var mainNav = new NavigationPage(new HomeScreen());
            MainPage = mainNav;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
