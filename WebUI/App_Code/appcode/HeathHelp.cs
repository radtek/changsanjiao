using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

/// <summary>
///SendHelper 的摘要说明
/// </summary>
public class HeathHelp
{
    public HeathHelp()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    public static string GetEmailHtml(string type, string userID, string title, List<string[]> infoes)
    {
        string html = GetHtml(title, infoes).Replace("<table cellspacing='1'", "<table style='width:80%;min-width:500px;'cellspacing='1'");
        html += "<p>如果您不想继续接受健康气象预报邮件，点击<a href=\"http://222.66.83.21:8282/CancelRequest/Cancel.ashx?type=" + type + "&userID=" + userID + "\" target=\"_blank\">取消订阅</a>。</p>";
        return html;
    }


    public static string GetHtml(string title, List<string[]> infoes)
    {
        string html2 = "";
        string imgUrl = "http://222.66.83.21:8282/CancelRequest/email/";
        //string imgUrl = "http://10.228.177.62:8282/CancelRequest/email/";
        string typeURL = "";
        string visibility = "";
        if (infoes[0][0].IndexOf("哮喘") >= 0)
        {
            typeURL = "etxc.png";
            visibility = "display:block";
        }
        else if (infoes[0][0].IndexOf("儿童感冒") >= 0)
        {
            typeURL = "etgm.png";
            visibility = "display:block";
        }
        else if (infoes[0][0].IndexOf("青少年和成年人感冒") >= 0)
        {
            typeURL = "qsn.png";
            visibility = "display:none";

        }
        else if (infoes[0][0].IndexOf("老年人感冒") >= 0)
        {
            typeURL = "lnr.png";
            visibility = "display:none";
        }
        else if (infoes[0][0].IndexOf("COPD") >= 0)
        {
            typeURL = "copd.png";
            visibility = "display:block";
        }
        else if (infoes[0][0].IndexOf("重污染") >= 0)
        {
            typeURL = "zwr.png";
            visibility = "display:block";
        }
        html2 += "<img width='101' height='70' src='" + (imgUrl + typeURL) + "'/><br><span style='color:black' align='center'>" + GetPeople(infoes[0][0]) + "</span>";

        //string html = "<table cellspacing='1' cellpadding='5' border='0' bgcolor='#e9e9e9' width='100%'><tr   bgcolor='#fafafa' style='color:#fff'><td rowspan='2' style='font-size:16px; font-family:微软雅黑; width:20%; text-align:center;'>" + html2 + "</td>";
        string html = "<div style='"+visibility+"'><h4 style='margin:15px 0 8px 0;;font-weight: bold;'>" + title + "</h4></div><table cellspacing='1' cellpadding='5' border='0' bgcolor='#e9e9e9' width='100%'>"
            + "<tr   bgcolor='#fafafa' style='color:#fff'><td rowspan='2' style='font-size:16px; font-family:微软雅黑; width:20%; text-align:center;'>" + html2 + "</td>";

        if (infoes[0][2].Contains("中暑"))
        {
            html = "<h4 style='margin:15px 0 8px 0;font-weight: bold;'>" + title + "</h4><table cellspacing='1' cellpadding='5' border='0' bgcolor='#e9e9e9' width='100%'>"
            + "<tr bgcolor='#1458d7' style='color:#fff'>";
        }

        for (int i = 0; i < infoes.Count; i++)
        {
            html += "<td style='font-size:16px; font-family:微软雅黑; width:40%; background-color:#1458d7 '><strong>" + infoes[i][1] + "</strong></td>";
        }

    

        if (!infoes[0][2].Contains("中暑"))
            html += "</tr><tr bgcolor='#fafafa' style='color:#000'>";
        else
            html += "</tr><tr bgcolor='#fafafa' style='color:#000'>";


        string type = (infoes[0][2].Contains("中暑") ? "SunLevel_" : "WarningLevel_");
        for (int i = 0; i < infoes.Count; i++)
        {
            if (infoes[0][2].Contains("中暑"))
            {
                html += "<td style='font-size:16px; font-family:微软雅黑;vertical-align: top;'><strong>风险等级：</strong><img src='" + imgUrl + type + GetLvl(infoes[i][2]) + "'><br><strong>防护建议：</strong>" + infoes[i][4] + "</td>";
            }
            else
            {
                html += "<td style='font-size:16px; font-family:微软雅黑;vertical-align: top;'><strong>风险等级：</strong><img src='" + imgUrl + type + GetLvl(infoes[i][2]) + "'><br><strong>防范人群：</strong>" + infoes[i][3] + "<br><strong>防护建议：</strong>" + infoes[i][4] + "</td>";
            }
        }
        //string fx = "气象风险图例";
        //if (infoes[0][0] == "COPD" || infoes[0][0] == "儿童哮喘")
        //{
        //    fx = "气象环境风险图例";
        //}

        //html += "</tr></table><p style='font-size:16px; font-family:微软雅黑; margin-top:20px; width:250px;'>" + infoes[0][0].Replace("青年感冒", "青少年和成年人感冒").Replace("老年感冒", "老年人感冒") + "" + fx + " </p><span style='margin-left:252px; position: absolute;margin-top: -35px;'>";
        //for (int i = 1; i < 6; i++)
        //{
        //    html += "<img src='" + imgUrl + type + i + ".png'/>";
        //}
        html += "</tr></table>";
        return html;
        //return html + "</span><p style='margin-top:20px'><a href='mailto:jkqx@smb.gov.cn' target='_blank'>请把jkqx@smb.gov.cn加到白名单或联系人中以确保能够正确接收此邮件。</a></p>";
    }

    public static string GetTitle(string day, string healthyType,string siteName,string forecastTime)
    {
       // return region + "气象台" + day + "发布" + healthyType + "气象风险预报（试行）";

        string ht = "青年感冒";
        switch (healthyType) {
            case "青年感冒": ht = "青少年和成年人感冒"; break;
            case "老年感冒": ht = "老年人感冒"; break;
            case "COPD患者": ht = "COPD"; return day + forecastTime + "时发布" + (siteName == "中心城区" ? "上海市" : siteName) + "" + ht + "气象环境风险预报";
            case "儿童哮喘": ht = "儿童哮喘"; return day + forecastTime + "时发布" + (siteName == "中心城区" ? "上海市" : siteName) + "" + ht + "气象环境风险预报";
            default: ht = "感冒"; break;
        }

        return  day +forecastTime+ "时发布" + (siteName=="中心城区"?"上海市":siteName) + "" + ht + "气象风险预报";
    }
    public static string GetTitle(string day, string reigon ,string healthyType, string siteName,string zhanweiyongde)
    {
        // return region + "气象台" + day + "发布" + healthyType + "气象风险预报（试行）";

        string ht = "青年感冒";
        switch (healthyType)
        {
            case "青年感冒": ht = "青少年和成年人感冒"; break;
            case "老年感冒": ht = "老年人感冒"; break;
            case "COPD": ht = "COPD"; return reigon+""+day + "发布" + (siteName == "中心城区" ? "上海市" : siteName) + "" + ht + "气象环境风险预报";
            case "儿童哮喘": ht = "儿童哮喘"; return reigon + "" + day + "发布" + (siteName == "中心城区" ? "上海市" : siteName) + "" + ht + "气象环境风险预报";
            default: ht = healthyType; break;
        }

        return reigon + "" + day + "发布" + (siteName == "中心城区" ? "上海市" : siteName) + "" + ht + "气象风险预报";
    }

    public static string GetMessage(string region,DateTime time,string healthyType,string lvl,string suggest,string siteName) {
       // string content = time.ToString("MM月dd日") + healthyType + "风险等级：" + lvl + "。" + suggest + region + "气象台" + time.ToString("MM/dd");


        string ht = "青年感冒";
        switch (healthyType)
        {
            case "青年感冒": ht = "青少年和成年人感冒"; break;
            case "老年感冒": ht = "老年人感冒"; break;
            default: ht = healthyType; break;
        }
        string fx = "气象风险";
        if (healthyType == "COPD" || healthyType == "儿童哮喘")
        {
            fx = "气象环境风险";
        }

        string content = time.ToString("M月dd日") + (siteName == "中心城区" ? "上海市" : siteName) + (ht + fx) + "预报等级：" + lvl + "。" + suggest + " " + region + " " + DateTime.Now.ToString("MM/dd");
        return content;
    }

    public static string GetMessageFTP(string region, DateTime time, string healthyType, string lvl, string suggest,string people)
    {
        // string content = time.ToString("MM月dd日") + healthyType + "风险等级：" + lvl + "。" + suggest + region + "气象台" + time.ToString("MM/dd");


        string ht = "青年感冒";
        switch (healthyType)
        {
            case "青年感冒": ht = "青少年和成年人感冒"; break;
            case "老年感冒": ht = "老年人感冒"; break;
            default: ht = healthyType; break;
        }
        string fx = "气象风险等级：";
        if (healthyType == "COPD" || healthyType == "儿童哮喘")
        {
            fx = "气象环境风险等级：";
        }
        string content = ht + "</br> " + time.ToString("yyyy-MM-dd") + "</br> " + fx + "" + lvl + "</br>防范人群：" + people + "" + "</br>预防建议：" + suggest;
        if (ht == "中暑")
        {
            content = ht + "</br>" + time.ToString("yyyy-MM-dd") + "</br>" + fx + "" + lvl + "</br>" + " 预防建议：" + suggest;
        }

        //string content = time.ToString("M月dd日") + (ht + fx) + "预报：" + lvl + "。" + suggest + " " + region + " " + DateTime.Now.ToString("MM/dd");
        return content;
    }

    private static string GetLvl(string lvl) {
        switch (lvl) {
            case "低": case "不易中暑": return "1.png";
            case "轻微": case "可能中暑": return "2.png";
            case "中等":  case "较易中暑": return "3.png";
            case "较高": case "容易中暑": return "4.png";
            case "高": case "极易中暑": return "5.png";
            default: return "";
        }
    }

    //private static string GetPeople(string type)
    //{
    //    switch (type)
    //    {
    //        case "儿童感冒": return "儿童：14岁以下";
    //        case "青年感冒": return "青少年和成年人：14-65岁";
    //        case "老年感冒": return "老年人：65岁以上";
    //        case "儿童哮喘": return "儿童哮喘患者";
    //        case "重污染": return "--";
    //        case "COPD": return "COPD患者";
    //        case "中暑": return "--";
    //        default: return "";
    //    }
    //}

    private static string GetPeople(string type)
    {
        switch (type)
        {
            case "儿童感冒": return "儿童<br/>14岁以下";
            case "青少年和成年人感冒": return "青少年和成年人<br/>14-65岁";
            case "老年人感冒": return "老年人<br/>65岁以上";
            case "儿童哮喘": return "儿童哮喘患者";
            case "重污染": return "--";
            case "COPD患者": return "COPD患者";
            case "中暑": return "--";
            default: return "";
        }
    }

}