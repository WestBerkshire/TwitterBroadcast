using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinqToTwitter;

namespace TwitterBroadcast
{
    public class emergencybroadcast
    {


        public static void Retweet()
        {

            var auth = new SingleUserAuthorizer
            {
                Credentials = new InMemoryCredentials
                {
                    ConsumerKey = System.Configuration.ConfigurationManager.AppSettings.Get("ConsumerKey").ToString(),
                    ConsumerSecret = System.Configuration.ConfigurationManager.AppSettings.Get("ConsumerSecret").ToString(),
                    OAuthToken = System.Configuration.ConfigurationManager.AppSettings.Get("OAuthToken").ToString(),
                    AccessToken = System.Configuration.ConfigurationManager.AppSettings.Get("AccessToken").ToString()
                }
            };



            string strSearchTag = System.Configuration.ConfigurationManager.AppSettings.Get("hashtags").ToString();


            using (var twitterCtx = new TwitterContext(auth))
            {


                var statusList =
                (from list in twitterCtx.List
                 where list.Type == ListType.Statuses &&
                       list.IncludeEntities == true &&
                       list.IncludeRetweets == false &&
                       list.OwnerScreenName == System.Configuration.ConfigurationManager.AppSettings.Get("account").ToString() &&
                       list.Slug == System.Configuration.ConfigurationManager.AppSettings.Get("list").ToString() // name of list to get statuses for
                 select list)
                 .First();

                HtmlString newline = new HtmlString("<br/>");

                HttpContext.Current.Response.Write("<head><head><body>"); 

                HttpContext.Current.Response.Write("Found " + statusList.Statuses.Count.ToString() + " tweets in total" + newline); 


                foreach (var tweet in statusList.Statuses)
                {



                    HttpContext.Current.Response.Write("<hr/>Looking at tweet from " + tweet.User.Name.ToString() + newline);
                    HttpContext.Current.Response.Write("Tweet has " + tweet.Entities.HashTagEntities.Count.ToString() + " hashtags" + newline); 
                    
                    if (tweet.Entities.HashTagEntities.Count > 0)
                    {

                   
                        
                        foreach (var hashtag in tweet.Entities.HashTagEntities)
                        {

                            if (hashtag.Tag == strSearchTag)
                            {

                                HttpContext.Current.Response.Write("Hashtag " + strSearchTag + " found" + newline);

                              

                                    var retweet = twitterCtx.Retweet(tweet.StatusID);

                                    HttpContext.Current.Response.Write("Retweeted " + tweet.StatusID.ToString() + newline); 
                      

                              
                               

                            }




                        }

                    }



                }

                HttpContext.Current.Response.Write("</body>");
            }

        }
    }
}