using System;
using System.Collections.Generic;
using System.Text;


public class DisplayNameAttribute : System.Attribute
{
    public string Name = null;

    public DisplayNameAttribute(string name)
    {
        this.Name = name;
    }
}
public class ColumnAttribute : System.Attribute
{
    public string Name = null;

    public ColumnAttribute(string fieldName)
    {
        this.Name = fieldName;
    }
}
//Table
public class TableAttribute : System.Attribute
{
    public string Name = null;

    public TableAttribute(string fieldName)
    {
        this.Name = fieldName;
    }
}