using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.Media.Playback;
using ConvivaUWP;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Conviva
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            WinSettings settings = new WinSettings();
            settings.setGatewayURL("https://mlb-test.testonly.conviva.com");
            var result = WinClient.init("36c5a9412da1c521ff9a432d0161c15839224313", settings);
            var contentInfo = new WinContentInfo();
            contentInfo.setStreamURL("http://vod-l3c-na.media.plus.espn.com/ps01/espn/event/2019/06/22/UFC_Fight_Night_Moicano_v_20190622_1561230416312/master_desktop_complete_aeng-trimmed.m3u8");
            contentInfo.setIsLive(0);
            contentInfo.setPlayerName("UWPMediaPlayer");


            var jsonInfo =
                "{\"med\":\"video\",\"state\":\"ON\",\"productType\":\"VOD\",\"locationName\":\"SWIFTSTACK_GLOBAL_LEVEL3\",\"cdnName\":\"LEVEL3\",\"pbs\":\"uwp-desktop~ssai\",\"fguid\":\"b88c1ae1-45bf-4fcc-84ae-58b301dfd77b\",\"prt\":\"espn\",\"conid\":\"315e9ff4-115b-4a1e-a46b-8c4e7f449129\",\"userid\":\"af76129b-630c-4df9-85b8-05fcc73da66f\",\"assetName\":\"UFC Fight Night: Moicano vs. The Korean Zombie (Main Card) - 4ac74ee4-81af-4da6-86cc-48a98638cac9\",\"plt\":\"uwp-desktop\",\"authType\":\"Direct\",\"duration\":\"9745\",\"eventId\":\"100026483\",\"network\":\"ESPN+\",\"trackingId\":\"4ac74ee4-81af-4da6-86cc-48a98638cac9\",\"contentId\":\"4ac74ee4-81af-4da6-86cc-48a98638cac9\",\"airingId\":\"UNKNOWN\",\"assetType\":\"Replay\"}";

            var tags = (Dictionary<string, string>) Newtonsoft.Json.JsonConvert.DeserializeObject(jsonInfo, typeof(Dictionary<string, string>));
            if (tags.TryGetValue("assetName", out string assetName))
            {
                contentInfo.setAssetName(assetName);
            }
            if (tags.TryGetValue("cdnName", out string cdn))
            {
                contentInfo.setDefaultCDN(cdn);
            }
            if (tags.TryGetValue("userid", out string userId))
            {
                contentInfo.setViewerId(userId);
            }
            else if (tags.TryGetValue("c3.viewer.id", out string viewerId))
            {
                contentInfo.setViewerId(viewerId);
            }

            foreach (var tag in tags)
            {
                contentInfo.setTag(tag.Key, tag.Value);
            }
            var version = Package.Current.Id.Version;
            contentInfo.setTag("ver", string.Join(".", version.Major, version.Minor, version.Build, version.Revision));
            var sessionId = WinClient.createSession(contentInfo);
            
            var playerProxy= new MediaPlayerProxy(MPlayer.MediaPlayer);
            WinClient.attachPlayer(sessionId, playerProxy);
        }
    }
}
