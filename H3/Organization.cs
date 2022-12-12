namespace H3.Organization
{
    public class User : Unit
    {  
        public static string SystemUserId { get;  set; }
        public static string AnonymousUserId { get; internal set; }
        public string  DepartmentName { get;  set; }
        public string FullName { get; internal set; }
        public string Name { get; internal set; }
        public string UnitId { get; internal set; }
    }
          

        public  class OrgRole
        {
            public string Name { get; internal set; }
            public string ObjectId { get; internal set; }
        }
        public class Company
        {
             public string CompanyId { get; set; }
        }

}