
namespace RhemaCMS.Models.MSTRModels
{
    public class UserPermissionMaster
    {
        public UserPermissionMaster()
        { }

        //Access rights can be granted at activity level or module level -- CREATE [add] ..READ [view] ..UPDATE [edit] ..DELETE[remove] ..MANAGE [configure]   --- about 80 permissions x 5   ~ 400 perms

        //System_Adminitration functions... @Vendor  begins with "__0"    [ ____ : (, ___ : ), ______ : - , _____ : &]
        //[ SYS (only creates Sup Admin), SUP_ADMN (Has all access srights), SYS_ADMN (selected /dedicated) ] ... SuperAdmin_Priv : begin with "__0"    [ ____ : (, ___ : ), __ : - ]
        public bool _A0__System_Administration { get; set; }  // 10
        public bool _A0_00__Super_Admin_Account { get; set; }  // SYS role -- SYS acc only
        public bool _A0_01__System_Administrator_Accounts { get; set; }
        public bool _A0_02__Church_Faith_Types { get; set; }   
        public bool _A0_03__Denominations { get; set; }
        public bool _A0_04__Church_Levels { get; set; }
        public bool _A0_05__Congregations { get; set; }
        public bool _A0_06__Subscribers_Unit_Church_Structure { get; set; }
        public bool _A0_07__Church_Administrator_Accounts { get; set; }
        public bool _A0_08__User_Subscriptions { get; set; }


        //Dashboard... SuperAdmin_Priv : begins with "_00"    [ ____ : (, ___ : ), ______ : - , _____ : &]
        public bool _00__Dashboard { get; set; }   // 8
        public bool _00_01__Oversight_Congregations { get; set; }
        public bool _00_02__Members { get; set; }
        public bool _00_03__New_Converts { get; set; }
        public bool _00_04__Receipts____Income___ { get; set; }
        public bool _00_05__Payments____Expense___ { get; set; }    
        public bool _00_06__Church_Attendance_Trend { get; set; }    
        public bool _00_07__To______Do_List { get; set; }



        //Subscribed App Configurations : being with "_01"   [ ____ : (, ___ : ), ______ : - , _____ : &]
        public bool _01__App_Configurations { get; set; }   // 9
        public bool _01_01__Church_Paramters____Configurations___ { get; set; } /* Calendar Year, Member Type, Member Status, Church Rank/Grade, Church Position, Church Service Category, Church Services, Church Sectors[groups, cttees etc.], Leader roles, Sector roles, Church-life Activities, Church Activity requirement, Church event categories, Church Unit Type, Configure_Transfer_Type_Church_Level, Approval_Process, Approval_Process_Step */
        public bool _01_02__General_Parameters { get; set; } /* Countries, Country Regions, Currencies, Units of Measure, Institution Types, Certificate Types, Relationship_Type, National_ID_Type*/
        public bool _01_03__Church_Units_Structure____Organisational_Chart___ { get; set; }  /* Governing Body,Church Office ,Department ,Church Grouping,Standing Committee, Church Enterprise, Team, Church Position, Independent Unit, Congregation  */                                                                          
        public bool _01_04__Internal_User_Accounts { get; set; }
        public bool _01_05__User_Preferences { get; set; }   //name value pair settings
        public bool _01_06__Import_Configuration { get; set; }   //based on level e.g.  HQ Creates config; Presbytery imports or creates new; District imports or create new; Congregation imports or create new.... check ChurchBodyOwner column
        public bool _01_07__Upload_Configuration { get; set; }   //based on level e.g.  HQ Creates config; Presbytery imports or creates new; District imports or create new; Congregation imports or create new.... check ChurchBodyOwner column
        public bool _01_08__Custom_Import_Configuration { get; set; }   // customize imported config


        // Church Register module : 02   :   [ ____ : (, ___ : ), ______ : - ]
        public bool _02__Member_Explorer { get; set; }  //9
        public bool _02_01__Member_Explorer { get; set; } //  spanning thru existing members
        public bool _02_01_01__Member_Profile { get; set; } 
        public bool _02_01_02__Member_Church______life { get; set; }
        public bool _02_02__New_Members { get; set; }         
        public bool _02_03__Past_Membership { get; set; }
        public bool _02_04__Profile_Card { get; set; }
        public bool _02_05__Lookup_Member { get; set; }
        public bool _02_06__Member_History { get; set; }


        // Church-life & Events module : 03  :  [ ____ : (, ___ : ), ______ : - , _____ : &]
        public bool _03__Church______life_____Events { get; set; }  // 12
        public bool _03_01__Church_Calendar____Almanac___ { get; set; }
        public bool _03_02__Events_Countdown { get; set; }
        public bool _03_03__Church_Service_Line______up { get; set; }
        public bool _03_04__Order_of_Service { get; set; }
        public bool _03_05__Preaching_Plan { get; set; }
        public bool _03_06__Church_Activity_Roster { get; set; }
        public bool _03_07__Minister_Schedule { get; set; }
        public bool _03_08__Church_Core_Activities { get; set; }
        public bool _03_09__Member______Activity_Checklist { get; set; }
        public bool _03_10__My_Calendar { get; set; }
        public bool _03_11__To______Do_List { get; set; } 


        // Church Administration module : 04 : [ ____ : (, ___ : ), ______ : - , _____ : & ]
        public bool _04__Church_Administration { get; set; }  //19
        public bool _04_01__Congregations { get; set; }
        public bool _04_02__Church_Units { get; set; }
        public bool _04_03__Leadership_Pool { get; set; }
        public bool _04_04__Church_Projects { get; set; }    //Sync_Projects_with_Finance  --- budgeting 
        public bool _04_05__Church_Attendance { get; set; }
        public bool _04_06__Church_Visitors { get; set; }
        public bool _04_07__New_Converts { get; set; }
        public bool _04_08__Church_Transfers { get; set; }   // Make_Transfer_Request, Acknowledge_Transfer_Request
        public bool _04_08_01__Member_Transfers { get; set; }
        public bool _04_08_02__Clergy_Transfers { get; set; }
        public bool _04_08_03__Role_Transfers { get; set; }
        public bool _04_08_04__Transfer_Requests_Approval { get; set; } 
        public bool _04_09__Promotions_____Demotions { get; set; }
        public bool _04_10__Notices_____Announcements { get; set; }
        public bool _04_11__Internal______Communication { get; set; }
        public bool _04_11_01__Broadcast_Notifications { get; set; }
        public bool _04_11_02__Send_Birthday_Anniversary_Messages { get; set; } 
        public bool _04_12__Assets_Register { get; set; } 


        // Finance Management 05 : [ ____ : (, ___ : ), ______ : - , _____ : & ]
        public bool _05__Finance_Management { get; set; }  //9
        public bool _05_01__Receipts____Income___ { get; set; }  
        public bool _05_02__Payments____Expense___ { get; set; } 
        public bool _05_03__Offertory____Collection___ { get; set; } 
        public bool _05_04__Tithes { get; set; } 
        public bool _05_05__Donations { get; set; } 
        public bool _05_06__Trial_Balance { get; set; } 
        public bool _05_07__Financial_Reports { get; set; } 
        public bool _05_08__Sync_Capitalized_Assets { get; set; }   //with Finance segment


        // Reports &  Analytics 05 : [ ____ : (, ___ : ), ______ : - , _____ : & ]  
        public bool _06__Reports_____Analytics { get; set; }  //4
        public bool _06_01__Church_Statistics { get; set; }
        public bool _06_02__Growth_Trends { get; set; }
        public bool _06_03__Adhoc_Analysis { get; set; }
 
    }
}
