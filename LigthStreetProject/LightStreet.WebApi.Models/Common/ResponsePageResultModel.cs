using System.Collections.Generic;

namespace LightStreet.WebAPI.Models.Common
{
    /// <summary>
    /// Response model for JQuery DataTable plugin.
    /// /// </summary>
    public class ResponsePageResultModel<T> where T : class
    {
        public ResponsePageResultModel(List<T> aaData,
            string sEcho,
            int iTotalDisplayRecords,
            int iTotalRecords)
        {
            this.aaData = new List<T>(aaData);
            this.sEcho = sEcho;
            this.iTotalDisplayRecords = iTotalDisplayRecords;
            this.iTotalRecords = iTotalRecords;
        }

        public IEnumerable<T> aaData { get; }
        public string sEcho { get; }
        public int iTotalDisplayRecords { get; }
        public int iTotalRecords { get; }
    }
}