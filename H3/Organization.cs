namespace H3.Organization
{
    public class User
    {  
        public static string SystemUserId { get;  set; }
        public string  DepartmentName { get;  set; }
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