using System;
using System.Web;
public class ReportBasePage : System.Web.UI.Page
{
    protected ReportData ReportDataObj { get; set; }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (HttpContext.Current != null)
            if (HttpContext.Current.Session["ReportData"] != null)
            {
                ReportDataObj = HttpContext.Current.Session["ReportData"] as ReportData;
                return;
            }
        ReportDataObj = new ReportData();
        CaptureRouteData(Page.Request);
    }


    private void CaptureRouteData(HttpRequest request)
    {
        var mode = (request.QueryString["rptmode"] + "").Trim();
        ReportDataObj.IsLocal = mode == "local" ? true : false;
        ReportDataObj.ReportName = request.QueryString["reportname"] + "";
        string dquerystr = request.QueryString["parameters"] + "";
        if (!String.IsNullOrEmpty(dquerystr.Trim()))
        {
            var param1 = dquerystr.Split(',');
            foreach (string pm in param1)
            {
                var rp = new Parameter();
                var kd = pm.Split('=');
                if (kd[0].Substring(0, 2) == "rp")
                {
                    rp.ParameterName = kd[0].Replace("rp", "");
                    if (kd.Length > 1) rp.Value = kd[1];
                    ReportDataObj.ReportParameters.Add(rp);
                }
                else if (kd[0].Substring(0, 2) == "dp")
                {
                    rp.ParameterName = kd[0].Replace("dp", "");
                    if (kd.Length > 1) rp.Value = kd[1];
                    ReportDataObj.DataParameters.Add(rp);
                }
            }
        }
    }
}