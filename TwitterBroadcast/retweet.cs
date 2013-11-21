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




                foreach (var tweet in statusList.Statuses)
                {

                    if (tweet.Entities.HashTagEntities.Count > 0)
                    {

                        foreach (var hashtag in tweet.Entities.HashTagEntities)
                        {

                            if (hashtag.Tag == strSearchTag)
                            {
                                try
                                {

                                    var retweet = twitterCtx.Retweet(tweet.StatusID);


                                }
                                catch { }

                            }




                        }

                    }



                }


            }

        }
    }
}