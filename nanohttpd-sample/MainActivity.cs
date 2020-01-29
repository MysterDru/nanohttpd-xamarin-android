using System;
using Android.App;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using FI.Iki.Elonen.Util;
using Java.Net;
using Java.Util;

namespace nanohttpd_sample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
		private HelloServer _server;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

			this._server = new HelloServer();
			this._server.Start();

			var textView = FindViewById<TextView>(Resource.Id.TextView);
			textView.Text = GetIpAddress();
		}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
		
		string GetIpAddress()
		{
			var AllNetworkInterfaces = Collections.List(Java.Net.NetworkInterface.NetworkInterfaces);
			var IPAddress = "";
			foreach (var interfaces in AllNetworkInterfaces)
			{
				if (!(interfaces as Java.Net.NetworkInterface).Name.Contains("eth0")) continue;

				var AddressInterface = (interfaces as Java.Net.NetworkInterface).InterfaceAddresses;
				foreach (var AInterface in AddressInterface)
				{
					if (AInterface.Broadcast != null)
						IPAddress = AInterface.Address.HostAddress;
				}
			}
			return IPAddress;


			//WifiManager wifiManager = (WifiManager)this.ApplicationContext.GetSystemService(Android.Content.Context.WifiService);
			//int ipAddress = wifiManager.ConnectionInfo.IpAddress;
			//var formatedIpAddress = Java.Lang.String.Format("%d.%d.%d.%d", (ipAddress & 0xff), (ipAddress >> 8 & 0xff), (ipAddress >> 16 & 0xff), (ipAddress >> 24 & 0xff));
			//return "http://" + formatedIpAddress + ":" + 8080;
		}

	}
}

