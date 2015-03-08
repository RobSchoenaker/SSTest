using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using ServiceStack;


namespace Ludu.SSTest
{
  [Activity(MainLauncher = true)]
  public class MainActivity : Activity
  {

    private static ServerEventsClient serverEventsClient = null;
    static ServerEventConnect connectMsg = null;
    static List<ServerEventMessage> msgs = new List<ServerEventMessage>();
    static List<ServerEventMessage> commands = new List<ServerEventMessage>();
    static List<System.Exception> errors = new List<System.Exception>();
    static string lastText = "";


    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.activity_main);
      ConnectServerEvents();
    }

    private void ConnectServerEvents()
    {
      if (serverEventsClient == null)
      {

        // var client = new ServerEventsClient("http://chat.servicestack.net", channels: "home")

        // bla.ybookz.com is a copy of the SS Chat sample, that sends 'HAHAHAHA' every second to all listeners

        serverEventsClient = new ServerEventsClient("http://bla.ybookz.com/", channels: "home")

        {

          OnConnect = e =>
          {
            connectMsg = e;
          },
          OnCommand = a =>
          {
            commands.Add(a);
          },
          OnHeartbeat = () =>
          {

            RunOnUiThread(() =>
            {
              try
              {
                Toast.MakeText(this, "Heartbeat", ToastLength.Short).Show();
              }
              catch
              {

              }
            });
          },
          OnMessage = a =>
          {
            msgs.Add(a);
            try
            {
              if (lastText != a.Data)
              {
                lastText = a.Data ?? "";
                RunOnUiThread(() =>
                {
                  try
                  {
                    Toast.MakeText(this, lastText, ToastLength.Short).Show();
                  }
                  catch
                  {

                  }
                });
              }
            }
            catch (System.Exception ex)
            {

            }
          },
          OnException = ex =>
          {
            errors.Add(ex);
          },
        };

        serverEventsClient.HeartbeatRequestFilter = (x) =>
        {
          Console.WriteLine("HeartbeatRequestFilter");
        };
        serverEventsClient.Start();

      }
    }


  }

}