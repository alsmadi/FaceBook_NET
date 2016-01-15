using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookSharp.Winforms;
using FacebookSharp.Schemas.Graph;
using FacebookSharp.Extensions;
using Facebook;
using System.Windows.Threading;

namespace FacebookSharp.Samples.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            FacebookSettings fbSettings = new FacebookSettings { ApplicationKey = txtApiKey.Text };
            FacebookLoginForm fbLoginDlg = new FacebookLoginForm(fbSettings);
            FacebookAuthenticationResult fbAuthResult;

            if (fbLoginDlg.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("You are logged in.");
                fbAuthResult = fbLoginDlg.FacebookAuthenticationResult;
                txtAccessToken.Text = fbAuthResult.AccessToken;
                txtExpiresIn.Text = fbAuthResult.ExpiresIn.ToString();
                btnGetMyInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("You must login inorder to access Facebook features.");
                if (fbLoginDlg.FacebookAuthenticationResult != null)
                {   // it can be null if the user just cancelled.
                    fbAuthResult = fbLoginDlg.FacebookAuthenticationResult;
                    MessageBox.Show(fbAuthResult.ErrorReasonText);
                }
            }
        }

        private object GetFriendsCount()
        {
            var fb = new FacebookClient(txtAccessToken.Text);
            //var fb = new FacebookClient();
            for (int i = 0; i < 100; i++) {
                dynamic result1 = fb.Get(i);
                string id = result1.id;
                string firstName = result1.first_name;
                var lastName = result1.last_name;
            }
            var query = string.Format("SELECT friend_count, name FROM user WHERE uid = me()");
            // var query1 = string.Format("SELECT uid,name,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})", "me()");
            fb.GetAsync("fql", new { q = query });
            object returnVal = 0;
            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                  //  Dispatcher.BeginInvoke();// () => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                var data = (IList<object>)result["data"];
                foreach (var comment in data.Cast<IDictionary<string, object>>())
                {
                    returnVal = comment["friend_count"];
                    // Dispatcher.BeginInvoke(() => MessageBox.Show(returnVal.ToString()));
                }

            };

            return returnVal;
        }
        private void btnGetMyInfo_Click(object sender, EventArgs e)
        {
            Facebook fb = new Facebook(txtAccessToken.Text);
            //var friends = fb.GetConnections("me", "friends", null);
            //  var friends = GetFriendsCount();

            for (int i = 4; i < 100; i++)
            {
                dynamic result1 = fb.Get(i.ToString());
                string id = result1.id;
                string firstName = result1.first_name;
                var lastName = result1.last_name;
            }

            //  FacebookCollection<object> abc = new FacebookCollection<object>();
            /*   
                // using async method */
            fb.GetAsync("/me",
                        result =>
                        {
                            // incase you are using async,
                            // always check if it was successful.
                            if (result.IsSuccessful)
                            {
                                // this prints out the raw json
                              //  MessageBox.Show(result.RawResponse);
                                string test = result.RawResponse;
            // this mite be preferable - the generic version of the result
            var user = result.GetResponseAs<User>();
                                var friends1 = fb.GetConnections(user.Name, "friends", null);
                                foreach (var friend in friends1)
                                {
                                    string abc = friend.ToString();
                                }
                                //Response.Write(friend["name"]);

                                //post to my wall
                                var data = new Dictionary<string, string>();
                                data.Add("message", "testing facebook graph api");
                                var putobject = fb.PutObject(user.Name, "feed", data);
                                MessageBox.Show("Hi " + user.Name);
                            }
                            else
                            {
                                // exception is stored in result.Exception
                                // u can extract the message using result.Exception.Message
                                // or u can get raw facebook json exception using result.Response.
                                MessageBox.Show("Error: " + Environment.NewLine + result.Exception.Message);
                            }
                        });

            // you can also use synchronous version like below
            // Methods not containing Async are treated as synchronous.

            // you can either use generic version or non generic version
            //var user = fb.Get<User>("/me");
            //MessageBox.Show("Hi " + user.Name);

            // non-generic version returns raw JSON string given by Facebook.
            //MessageBox.Show(fb.Get("/me"));

            // example for posting on the wall:
            //string resultPost = fb.Post("/me/feed", new Dictionary<string, string>
            //                        {
            //                            {"message", "testing from FacebookSharp."}
            //                        });
            //MessageBox.Show(resultPost); // this result is the id of the new post

            // example for deleting
            //string resultDelete = fb.Delete("/id");
            //MessageBox.Show(resultDelete);

            // you can also use Parameter Extensions
            // (make sure you add -> using FacebookSharp.Extensions; )
            // https://graph.facebook.com/?fields=id,picture&ids=123741737666932,100001241534829&oauth_token=your_oauth_token.
            // var result = fb.Get(string.Empty,
            //                    new Dictionary<string, string>()
            //                        .SelectFields(new[] { "picture" })
            //                        .SelectIds(new[] { "123741737666932", "100001241534829" })
            //                        .SelectField("id"));
            // You can also do things like .Offset(2).LimitTo(3) and much more.
            // checkout ParameterExtensions.cs file


            // You can also use the old RestApi by calling GetUsingRestApi or GetUsingRestApiAsync
            fb.GetUsingRestApiAsync("status.get", new Dictionary<string, string>().LimitTo(1),
                                    callback =>
                                    {
                                        if (callback.IsSuccessful)
                                        {
                                            MessageBox.Show(callback.RawResponse);
                                        }
                                    });
            // You can also use the Facebook Query Language by calling Query or QueryAsync methods.
            fb.QueryAsync("SELECT name FROM user WHERE uid = me()",
                          result =>
                          {
                              if (result.IsSuccessful)
                              {
                                  MessageBox.Show("Response using FQL: " + result.RawResponse);
                              }
                          });
        }
    }
}
