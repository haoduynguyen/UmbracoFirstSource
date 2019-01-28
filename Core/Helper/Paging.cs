using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System;
using System.Web.Mvc;

namespace Customs.Helper
{
    public class Paging
    {
        public string NextClass = "next";
        public string PrevClass = "prev";
        public string Next = "Next";
        public string Prev = "Previous";
        public string First = "none";
        public string Last = "none";
        public int maxpage = 4;
        public string EventClick = "";
        public string Sperator = "";
        public string searchtext = "";
        public string BaseUrl = "";
        public string Current = "<a class=\"js-active\">%page%</a>";
        public string ClassItem="";
        public string SeparatorLast = "<a class='separator-last'>...</a>";
        public string Multi(int num, int perpage, int curpage, string mpurl)
        {
            curpage = curpage <= 0 ? 1 : curpage;
            int page = maxpage;
            string multipage = "";
            int realpages = 1;
            int from = 0, to = 1;
            if (num > perpage)
            {
                int offset = 2;
                realpages = num / perpage + ((num % perpage > 0) ? 1 : 0);
                int pages = realpages;
                if (page > pages)
                {
                    from = 1;
                    to = pages;
                }
                else
                {
                    from = curpage - offset;
                    to = from + page - 1;
                    if (from < 1)
                    {
                        to = curpage + 1 - from;
                        from = 1;
                        if (to - from < page)
                        {
                            to = page;
                        }
                    }
                    else if (to > pages)
                    {
                        from = pages - page + 1;
                        to = pages;
                    }
                }
                int prevpage = from - 1;
                if (Prev == "")
                {
                    Prev = prevpage.ToString();
                }
                multipage += (this.First != "none"
                    ? "<li class=\"" + PrevClass + "\"><a " + ReplacePage(EventClick, 1) + " target=\"_self\" href=\"" + ReplacePage(mpurl, 1) + "\">" + First + "</a></li>" 
                    : "");


                //multipage += curpage - offset > 1 && pages > page && pages > maxpage ? "<a class=\"" + ClassItem + "\" href=\"" + ReplacePage(mpurl, 1) + "\">1</a></li>" + SeparatorLast : "";
                for (int i = from; i <= to; i++)
                {
                    multipage += i == curpage 
                        ? ReplacePage(this.Current, i) + (i < to ? Sperator : "")
                        : "<li><a " + ReplacePage(EventClick, i) + " target=\"_self\" href=\"" + ReplacePage(mpurl, i) + "\">" + i + "</a>" + (i == to ? "" : Sperator);
                }
               // multipage += (curpage < pages && pages > maxpage ? SeparatorLast + "<li><a class=\"" + ClassItem + "\" href=\"" + ReplacePage(mpurl, pages) + "\" target=\"_self\">" + realpages + "</a></li>" : "");
                int nextpage = to + 1;
                if (Next == "")
                {
                    Next = nextpage.ToString();
                }
                multipage += 
                    ((this.Last != "none")
                    ? "<li class=\"" + NextClass + "\"><a " + ReplacePage(EventClick, pages) + " target=\"_self\" href=\"" + ReplacePage(mpurl, pages) + "\">" + Last + "</a></li>" 
                    : "");
                multipage += "";
            }
            return multipage;
        }
        public string ReplacePage(string url, int page)
        {
            url = url.Replace("%page%", page.ToString());
            url = url.Replace("%25page%25", page.ToString());
            return url;
        }

        public static void ParseIndex(int page, int total, out int index, out int end)
        {
            index = page == 1 ? 0 : ((page - 1) * total) + 1;
            end = page == 1 ? total : (index - 1) + total;
        }
    }
}