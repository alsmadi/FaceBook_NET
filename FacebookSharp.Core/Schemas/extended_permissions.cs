namespace FacebookSharp.Schemas
{
    using System;

    /// <summary>
    /// Extended permissions for facebook.
    /// </summary>
    /// <remarks>
    /// http://developers.facebook.com/docs/authentication/permissions
    /// </remarks>
    [Flags]
    public enum ExtendedPermissions : long
    {
        #region Facebook# specifics

        // these are nothing related with facebook.com
        // just facebook# apis to make it better

        [StringValue("")]
        None = 1,
        [StringValue("")]
        All = 2,

        #endregion

        #region Publishing Permissions
        [StringValue("publish_stream")]
        PublishStream = 4,
        [StringValue("create_event")]
        CreateEvent = 8,
        [StringValue("rsvp_event")]
        RsvpEvent = 16,
        [StringValue("sms")]
        Sms = 32,
        [StringValue("offline_access")]
        OfflineAccess = 64,
        #endregion

        #region  Page Permissions
        [StringValue("manage_pages")]
        ManagePages = 128,
        #endregion

        #region Data Permissions
        [StringValue("email")]
        Email = 256,

        [StringValue("read_insights")]
        ReadInsights = 512,

        [StringValue("read_stream")]
        ReadStream = 1024,

        [StringValue("read_mailbox")]
        ReadMailbox = 2048,

        [StringValue("ads_management")]
        AdsManagement = 4096,

        [StringValue("xmpp_login")]
        XmppLogin = 8192,

        [StringValue("user_about_me")]
        UserAboutMe = 16384,
        [StringValue("friends_activities")]
        FriendsActivities = 32768,

        [StringValue("user_birthday")]
        UserBirthday = 65536,
        [StringValue("friends_birthday")]
        FriendsBirthday = 131072,

        [StringValue("user_education_history")]
        UserEducationHistory = 262144,
        [StringValue("friends_education_history")]
        FriendsEducationHistory = 524288,

        [StringValue("user_events")]
        UserEvents = 1048576,
        [StringValue("friends_events")]
        FriendsEvents = 2097152,

        [StringValue("user_groups")]
        UserGroups = 4194304,
        [StringValue("friends_groups")]
        FriendsGroups = 8388608,

        [StringValue("user_hometown")]
        UserHometown = 16777216,
        [StringValue("friends_hometown")]
        FriendsHometown = 33554432,

        [StringValue("user_interests")]
        UserInterests = 67108864,
        [StringValue("friends_interests")]
        FriendsInterests = 134217728,

        [StringValue("user_likes")]
        UserLikes = 268435456,
        [StringValue("friends_likes")]
        FriendsLikes = 536870912,

        [StringValue("user_location")]
        UserLocation = 1073741824,
        [StringValue("friends_location")]
        FriendsLocation = 2147483648,

        [StringValue("user_notes")]
        UserNotes = 4294967296,
        [StringValue("friends_notes")]
        FriendsNotes = 8589934592,

        [StringValue("user_online_presense")]
        UserOnlinePresense = 17179869184,
        [StringValue("friends_online_presense")]
        FriendsOnlinePresense = 34359738368,

        [StringValue("user_photo_video_tags")]
        UserPhotoVideoTags = 68719476736,
        [StringValue("friends_photo_video_tags")]
        FriendsPhotoVideoTags = 137438953472,

        [StringValue("user_photos")]
        UserPhotos = 274877906944,
        [StringValue("friends_photos")]
        FriendsPhotos = 549755813888,

        [StringValue("user_relationships")]
        UserRelationships = 1099511627776,
        [StringValue("friends_relationships")]
        FriendsRelationships = 2199023255552,

        [StringValue("user_religion_politics")]
        UserReligionPolitics = 4398046511104,
        [StringValue("friends_religion_politics")]
        FriendsReligionPolitics = 8796093022208,

        [StringValue("user_status")]
        UserStatus = 17592186044416,
        [StringValue("friends_status")]
        FriendsStatus = 35184372088832,

        [StringValue("user_videos")]
        UserVideos = 70368744177664,
        [StringValue("friends_videos")]
        FriendsVideos = 140737488355328,

        [StringValue("user_website")]
        UserWebsite = 281474976710656,
        [StringValue("friends_website")]
        FriendsWebsite = 562949953421312,

        [StringValue("user_work_history")]
        UserWorkHistory = 1125899906842624,
        [StringValue("friends_work_history")]
        FriendsWorkHistory = 2251799813685248,

        [StringValue("read_friendslists")]
        ReadFriendslists = 4503599627370496,

        [StringValue("read_requests")]
        ReadRequests = 9007199254740992

        #endregion
    }
}