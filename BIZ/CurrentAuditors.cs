using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using H3;
using H3.DataModel;


public class CurrentAuditors
{
    //private IEngine Engine;

    CurrentAuditors()
    {

    }


    public static void SetAuditors(IEngine Engine, string InstanceId, string SchemaCode, string BizObjectId)
    {
        DataTable activeinfo = Engine.Query.QueryWorkItemDisplayAndParticipant(new string[] { InstanceId },
            WorkItemState.Unfinished);

        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
        {

            string[] persons = new string[activeinfo.Rows.Count];
            int i = 0;
            foreach (DataRow row in activeinfo.Rows)
            {
                persons[i] += row["Participant"] + string.Empty + ",";
                i += 1;
            }
            BizObject currentObj = Tools.BizOperation.Load(Engine, SchemaCode, BizObjectId);
            currentObj["CurrentAuditors"] = GetParticipantsBy(Engine,persons);
            currentObj.Update();
        }
    }
    static string GetParticipantsBy(IEngine Engine,string[] strNames)
    {
        string users = "";
        //string[] strNames = (string[]) this.Request.BizObject[Roughing.Worker];//as string;   

        if (strNames.Length > 0)
        {
            foreach (string p in strNames)
            {
                DataRow dr = GetRow(Engine,"User", "ObjectId='" + p + "'");
                users += dr["Name"] + "，";
            }
        }
        return users;
    }
    static private DataRow GetRow(IEngine Engine,string table, string where, string selector = "*")
    {
        string sql = "select " + selector + " from " + "H_" + table + (where == "" ? "" : " where " + where);

        DataTable dt = Engine.Query.QueryTable(sql, null);
        int Count = dt.Rows.Count;
        if (Count > 0)
        {
            return dt.Rows[0];
        }
        else
        {
            return null;
        }
    }

}


